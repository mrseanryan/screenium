using System;
using System.Collections.Generic;

namespace screenium
{
    class TestRunner
    {
        internal void RunTests(List<TestDescription> testsToRun, ArgsProcessor argProc)
        {
            if (argProc.IsOptionOn(ArgsProcessor.Options.KeepOpenAfterRun) ||
                argProc.IsOptionOn(ArgsProcessor.Options.Save))
            {
                //TODO implement me
                throw new NotImplementedException();
            }

            foreach (var test in testsToRun)
            {
                Outputter.Output("Running test " + test.Name + " - " + test.Description);

                //TODO add using
                BrowserDriver driver = new BrowserDriver();
                driver.OpenUrl(test.Url, test.DivSelector, test.TitleContains);

                var dirManager = new DirectoryManager(argProc);

                string tempFilePath;

                tempFilePath = dirManager.GetTempFileName(test);
                driver.SaveDivImageToPath(test.DivSelector, tempFilePath);

                var comparer = new ImageComparer();
                CompareResult compareResult = comparer.CompareImages(tempFilePath, dirManager.GetExpectedImageFilePath(test), test.Name);
                ReportCreator reporter = new ReportCreator();

                Report report = reporter.CreateReport(compareResult, argProc.GetArg(ArgsProcessor.Args.OUTPUT_FILE_PATH));
                reporter.ShowReport(report);
            }
        }
    }
}
