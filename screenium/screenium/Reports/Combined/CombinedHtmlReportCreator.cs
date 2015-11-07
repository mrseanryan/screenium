//original license: MIT
//
//See the file license.txt for copying permission.

using System.Linq;
using screenium.Compare;
using screenium.Reports.Html;
using System.Collections.Generic;
using System;

namespace screenium.Reports
{
    /// <summary>
    /// creates/appends a combined summary that we append to at each run.
    /// </summary>
    class CombinedHtmlReportCreator
    {
        private CombinedReportPersistance _combinedPersist;
        private HtmlSupport _htmlSupport = new HtmlSupport();

        internal void CreateReport(ArgsProcessor argProc)
        {
            var dirManager = new DirectoryManager(argProc);
            _combinedPersist = new CombinedReportPersistance(dirManager);

            var combined = _combinedPersist.LoadCombinedData();

            var template = new TemplateCreator(argProc, TemplateCreator.TemplateNameCombinedReport);

            SetHeader(combined, template);

            string suitesHtml = GetSuitesHtml(combined.ReportSets);
            template.SetTemplateParam(TemplateCreator.TemplateParamCombinedSuiteParts, suitesHtml);

            var outPath = dirManager.GetCombinedReportHtmlPath();
            template.Save(outPath);
        }

        private string GetSuitesHtml(List<ReportSet> reportSets)
        {
            //TODO output the suites
            return "Not Implemented";
        }

        private void SetHeader(CombinedReportData combined, TemplateCreator template)
        {
            template.SetTemplateParam(TemplateCreator.TemplateParamCombinedDuration, DateSupport.ToString(combined.Duration));

            template.SetTemplateParam(TemplateCreator.TemplateParamCombinedResult, GetResultsAsHtml(combined.OverallResults));

            template.SetTemplateParam(TemplateCreator.TemplateParamCombinedStartTime, DateSupport.ToString(combined.StartTime));

            template.SetTemplateParam(TemplateCreator.TemplateParamCombinedTestsPassed, combined.TotalPassed.ToString());

            template.SetTemplateParam(TemplateCreator.TemplateParamCombinedTestsTotal, combined.TotalTests.ToString());
        }

        private string GetResultsAsHtml(List<CompareResult> results)
        {
            var hasBadResults = results.Any(res => res != CompareResult.Similar);

            var text = string.Join(", ", results);

            return _htmlSupport.GetHtmlColoredForResult(hasBadResults ? 
                CompareResult.Different : CompareResult.Similar, text);
        }
    }
}
