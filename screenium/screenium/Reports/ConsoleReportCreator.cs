//Copyright (c) 2015 Sean Ryan
//
//See the file license.txt for copying permission.

using System;

namespace screenium
{
    class ConsoleReportCreator : IReportCreator
    {
        Report IReportCreator.CreateReport(TestDescription test, CompareResultDescription compareResult)
        {
            var report = new Report
            {
                Result = compareResult,
                Test = test
            };

            return report;
        }

        bool IReportCreator.HasSaveCapability()
        {
            return false;
        }

        void IReportCreator.SaveReport(Report report, string filePath)
        {
            throw new NotSupportedException();
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
