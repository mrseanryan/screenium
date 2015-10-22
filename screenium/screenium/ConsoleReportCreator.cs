//Copyright (c) 2015 Sean Ryan
//
//See the file license.txt for copying permission.

using System;

namespace screenium
{
    class ConsoleReportCreator : IReportCreator
    {
        Report IReportCreator.CreateReport(TestDescription test, CompareResultDescription compareResult, string filePath)
        {
            var report = new Report
            {
                Result = compareResult,
                FilePath = null,
                Test = test
            };

            return report;
        }

        void IReportCreator.ShowReport(Report report)
        {
            Outputter.OutputEmphasised("Test Results:");

            Outputter.Output(report.Test.Name);

            Outputter.Output("Result: " + report.Result.Result);
            Outputter.Output("Tolerance: " + report.Result.Tolerance);
            Outputter.Output("Distortion: " + report.Result.Distortion);
        }
    }
}
