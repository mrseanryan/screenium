//original license: MIT
//
//See the file license.txt for copying permission.

namespace screenium.Reports
{
    /// <summary>
    /// creates/appends a combined summary that we append to at each run.
    /// </summary>
    class CombinedHtmlReportCreator
    {
        private CombinedReportPersistance _combinedPersist;

        internal void CreateReport(ArgsProcessor argProc)
        {
            var dirManager = new DirectoryManager(argProc);
            _combinedPersist = new CombinedReportPersistance(dirManager);

            var combined = _combinedPersist.LoadCombinedData();

            var template = new TemplateCreator(argProc, TemplateCreator.TemplateNameCombinedReport);

            template.SetTemplateParam(TemplateCreator.TemplateParamCombinedDuration, DateSupport.ToString(combined.Duration));

            //TODO color the results in divs
            template.SetTemplateParam(TemplateCreator.TemplateParamCombinedResult, string.Join(", ", combined.OverallResults));

            template.SetTemplateParam(TemplateCreator.TemplateParamCombinedStartTime, DateSupport.ToString(combined.StartTime));

            template.SetTemplateParam(TemplateCreator.TemplateParamCombinedTestsPassed, combined.TotalPassed.ToString());

            template.SetTemplateParam(TemplateCreator.TemplateParamCombinedTestsTotal, combined.TotalTests.ToString());

            //TODO output the suites

            var outPath = dirManager.GetCombinedReportHtmlPath();
            template.Save(outPath);
        }
    }
}
