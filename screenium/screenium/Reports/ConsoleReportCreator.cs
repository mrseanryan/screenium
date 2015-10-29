//original license: MIT
//
//See the file license.txt for copying permission.

using System;

namespace screenium.Reports
{
    class ConsoleReportCreator : IReportCreator
    {
        bool IReportCreator.HasSaveCapability()
        {
            return false;
        }

        void IReportCreator.SaveReport(ReportSet reports, ArgsProcessor argProc)
        {
            throw new NotSupportedException();
        }

        void IReportCreator.ShowReport(ReportSet reports)
        {
            foreach (var report in reports.Reports)
            {
                ShowReport(report);
            }
        }

        private void ShowReport(Report report)
        {
            Outputter.Output("");
            Outputter.OutputEmphasised("Test Results:");

            Outputter.Output("Test: " + report.Test.Name);
            Outputter.Output("Result: " + report.Result.Result, Compare.CompareResultHelper.GetResultAsConsoleColor(report.Result.Result));
            Outputter.Output("Tolerance: " + report.Result.Tolerance);
            Outputter.Output("Distortion: " + report.Result.Distortion);
            Outputter.Output("");
        }
    }
}
