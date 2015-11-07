//original license: MIT
//
//See the file license.txt for copying permission.

using screenium.Compare;
using screenium.Reports.Html;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace screenium.Reports
{
    /// <summary>
    /// creates/appends a combined summary that we append to at each run.
    /// </summary>
    class CombinedHtmlReportCreator
    {
        private CombinedReportPersistance _combinedPersist;
        private HtmlSupport _htmlSupport = new HtmlSupport();
        private ArgsProcessor _argProc;

        internal void CreateReport(ArgsProcessor argProc)
        {
            _argProc = argProc;
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
            StringBuilder sb = new StringBuilder();
            foreach (var set in reportSets)
            {
                sb.AppendLine(GetSuitesHtml(set));
            }

            return sb.ToString();
        }

        private string GetSuitesHtml(ReportSet set)
        {
            var template = new TemplateCreator(_argProc, TemplateCreator.TemplateNameCombinedReportTestPart);

            template.SetTemplateParam(TemplateCreator.TemplateParamSuiteName, set.CsvFileName);
            template.SetTemplateParam(TemplateCreator.TemplateParamSuiteReportLink,
                _htmlSupport.GetLink("view report", Path.GetFileName(set.FilePath)));
            template.SetTemplateParam(TemplateCreator.TemplateParamSuiteResult,
                _htmlSupport.GetHtmlColoredForResult(set.OverallResult, set.OverallResult.ToString()));
            template.SetTemplateParam(TemplateCreator.TemplateParamSuiteTestsPassed, set.CountTestsPassed);
            template.SetTemplateParam(TemplateCreator.TemplateParamSuiteTestsTotal, set.CountTests);

            return template.ToString();
        }

        private void SetHeader(CombinedReportData combined, TemplateCreator template)
        {
            template.SetTemplateParam(TemplateCreator.TemplateParamCombinedDuration, combined.Duration);

            template.SetTemplateParam(TemplateCreator.TemplateParamCombinedResult, GetResultsAsHtml(combined.OverallResults));

            template.SetTemplateParam(TemplateCreator.TemplateParamCombinedStartTime, combined.StartTime);

            template.SetTemplateParam(TemplateCreator.TemplateParamCombinedTestsPassed, combined.TotalPassed);

            template.SetTemplateParam(TemplateCreator.TemplateParamCombinedTestsTotal, combined.TotalTests);
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
