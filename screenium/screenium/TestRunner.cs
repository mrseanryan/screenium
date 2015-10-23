﻿//Copyright (c) 2015 Sean Ryan
//
//See the file license.txt for copying permission.

using System;
using System.Collections.Generic;
using screenium.Compare;
using screenium.Reports;

namespace screenium
{
    class TestRunner
    {
        internal void RunTests(List<TestDescription> testsToRun, ArgsProcessor argProc)
        {
            if (argProc.IsOptionOn(ArgsProcessor.Options.KeepOpenAfterRun))
            {
                //TODO implement me
                throw new NotImplementedException();
            }

            foreach (var test in testsToRun)
            {
                Outputter.Output("Running test " + test.Name + " - " + test.Description);

                using (var driver = new BrowserDriver())
                {
                    driver.OpenUrl(test.Url, test.DivSelector, test.TitleContains);

                    var dirManager = new DirectoryManager(argProc);

                    if (argProc.IsOptionOn(ArgsProcessor.Options.Run))
                    {
                        CompareActualPageVersusExpected(argProc, dirManager, test, driver);
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
        }

        private void SaveExpectedPage(DirectoryManager dirManager, TestDescription test, BrowserDriver driver)
        {
            Outputter.Output("Saving expected page for test: " + test.Name);
            driver.SaveDivImageToPath(test.DivSelector, dirManager.GetExpectedImageFilePath(test));
        }

        private void CompareActualPageVersusExpected(ArgsProcessor argProc, DirectoryManager dirManager,
            TestDescription test, BrowserDriver driver)
        {
            string tempFilePath = dirManager.GetActualImageFilePath(test);
            driver.SaveDivImageToPath(test.DivSelector, tempFilePath);

            var comparer = new CustomImageComparer(argProc);
            var compareResult = comparer.CompareImages(tempFilePath, dirManager.GetExpectedImageFilePath(test), test.Name);

            CreateReports(argProc, test, compareResult);
        }

        private void CreateReports(ArgsProcessor argProc, TestDescription test, CompareResultDescription compareResult)
        {
            var reporters = ReportCreatorFactory.CreateReporters();
            foreach (var reporter in reporters)
            {
                var report = CreateReport(test, compareResult);
                if (reporter.HasSaveCapability())
                {
                    reporter.SaveReport(report, argProc);
                }
                reporter.ShowReport(report);
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
