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
        internal CompareResult RunTests(List<TestDescription> testsToRun, ArgsProcessor argProc)
        {
            CompareResult overallResult = CompareResult.Similar;
            var reportSet = new ReportSet();
            foreach (var test in testsToRun)
            {
                Outputter.Output("Running test: " + test.Name + " - " + test.Description);

                using (var driver = new BrowserDriver())
                {
                    driver.SetWindowSize(test.WindowSize);

                    driver.OpenUrl(test.Url, test.DivSelector, test.TitleContains);

                    var dirManager = new DirectoryManager(argProc);

                    if (argProc.IsOptionOn(ArgsProcessor.Options.Run))
                    {
                        var reportThisTest = CompareActualPageVersusExpected(argProc, dirManager, test, driver);
                        var resultThisTest = reportThisTest.Result.Result;
                        if (resultThisTest != CompareResult.Similar)
                        {
                            overallResult = resultThisTest;
                        }
                        reportSet.Reports.Add(reportThisTest);
                    }
                    else if (argProc.IsOptionOn(ArgsProcessor.Options.Save))
                    {
                        SaveExpectedPage(dirManager, test, driver);
                    }
                    else
                    {
                        throw new InvalidOperationException("Not a support set of options");
                    }
                }
            }

            if (argProc.IsOptionOn(ArgsProcessor.Options.Run))
            {
                CreateReports(argProc, reportSet);
            }
            return overallResult;
        }

        private void SaveExpectedPage(DirectoryManager dirManager, TestDescription test, BrowserDriver driver)
        {
            Outputter.Output("Saving expected page for test: " + test.Name);
            driver.SaveDivImageToPath(test.DivSelector, dirManager.GetExpectedImageFilePath(test), test.CropAdjustWidth, test.CropAdjustHeight);
        }

        private Report CompareActualPageVersusExpected(ArgsProcessor argProc, DirectoryManager dirManager,
            TestDescription test, BrowserDriver driver)
        {
            string tempFilePath = dirManager.GetActualImageFilePath(test);
            driver.SaveDivImageToPath(test.DivSelector, tempFilePath, test.CropAdjustWidth, test.CropAdjustHeight);

            var comparer = new CustomImageComparer(argProc);
            var compareResult = comparer.CompareImages(tempFilePath, dirManager.GetExpectedImageFilePath(test), test);

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
