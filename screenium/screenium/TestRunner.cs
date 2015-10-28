//Copyright (c) 2015 Sean Ryan
//
//See the file license.txt for copying permission.

using screenium.Compare;
using screenium.Reports;
using screenium.SeleniumIntegration;
using System;
using System.Collections.Generic;

namespace screenium
{
    class TestRunner
    {
        BrowserDriver _driver;

        internal CompareResult RunTests(List<TestDescription> testsToRun, ArgsProcessor argProc)
        {
            using (_driver = new BrowserDriver())
            {
                CompareResult overallResult = CompareResult.Similar;
                var reportSet = new ReportSet();
                foreach (var test in testsToRun)
                {
                    Outputter.Output("Running test: " + test.Name + " - " + test.Description);
                    overallResult = RunTest(argProc, overallResult, reportSet, test);
                    Outputter.OutputSeparator();
                }

                if (argProc.IsOptionOn(ArgsProcessor.Options.Run))
                {
                    CreateReports(argProc, reportSet);
                }
                return overallResult;
            }
            _driver = null;
        }

        private CompareResult RunTest(ArgsProcessor argProc, CompareResult overallResult, ReportSet reportSet, TestDescription test)
        {
            _driver.SetWindowSize(test.WindowSize);

            _driver.OpenUrl(test.Url, test.DivSelector, test.TitleContains);

            var dirManager = new DirectoryManager(argProc);

            if (argProc.IsOptionOn(ArgsProcessor.Options.Run))
            {
                var reportThisTest = CompareActualPageVersusExpected(argProc, dirManager, test, _driver);
                var resultThisTest = reportThisTest.Result.Result;
                if (resultThisTest != CompareResult.Similar)
                {
                    overallResult = resultThisTest;
                }
                reportSet.Reports.Add(reportThisTest);
            }
            else if (argProc.IsOptionOn(ArgsProcessor.Options.Save))
            {
                SaveExpectedPage(dirManager, test, _driver);
            }
            else
            {
                throw new InvalidOperationException("Not a supported set of options");
            }
            return overallResult;
        }

        private void SaveExpectedPage(DirectoryManager dirManager, TestDescription test, BrowserDriver driver)
        {
            Outputter.Output("Saving expected page for test: " + test.Name);
            driver.SaveDivImageToPath(test.DivSelector, dirManager.GetExpectedImageFilePath(test), test.CropAdjustWidth, test.CropAdjustHeight, test.SleepTimespan);
        }

        private Report CompareActualPageVersusExpected(ArgsProcessor argProc, DirectoryManager dirManager,
            TestDescription test, BrowserDriver driver)
        {
            string actualFilePath = dirManager.GetActualImageFilePath(test);
            driver.SaveDivImageToPath(test.DivSelector, actualFilePath, test.CropAdjustWidth, test.CropAdjustHeight, test.SleepTimespan);

            var comparer = new CustomImageComparer(argProc);
            var compareResult = comparer.CompareImages(actualFilePath, dirManager.GetExpectedImageFilePath(test), test);

            var report = CreateReport(test, compareResult);

            return report;
        }

        private void CreateReports(ArgsProcessor argProc, ReportSet reports)
        {
            var reporters = ReportCreatorFactory.CreateReporters();
            foreach (var reporter in reporters)
            {
                if (reporter.HasSaveCapability())
                {
                    reporter.SaveReport(reports, argProc);
                }
                reporter.ShowReport(reports);
            }
        }

        private Report CreateReport(TestDescription test, CompareResultDescription compareResult)
        {
            var report = new Report
            {
                Result = compareResult,
                Test = test
            };

            return report;
        }
    }
}
