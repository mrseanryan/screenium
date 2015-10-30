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
            //in console output, is actually better to have summary at the *end*
            ShowReportHeader(reports);
        }

        private void ShowReportHeader(ReportSet reports)
        {
            Outputter.Output("");
            Outputter.OutputEmphasised("Test Results Summary:", ConsoleColor.Cyan);
            Outputter.Output("Filename: " + reports.CsvFileName);
            Outputter.Output("Duration: " + DateSupport.ToString(reports.Duration));

            Outputter.Output("Overall Result: " + reports.OverallResult, Compare.CompareResultHelper.GetResultAsConsoleColor(reports.OverallResult));
            Outputter.Output(reports.CountTestsPassed + " of " + reports.CountTests + " tests passed.");
        }

        private void ShowReport(Report report)
        {
            Outputter.Output("");
            Outputter.OutputEmphasised("Test Result:");

            Outputter.Output("Test: " + report.Test.Name);
            Outputter.Output("Result: " + report.Result.Result, Compare.CompareResultHelper.GetResultAsConsoleColor(report.Result.Result));
            Outputter.Output("Tolerance: " + report.Result.Tolerance);
            Outputter.Output("Distortion: " + report.Result.Distortion);
            Outputter.Output("");
        }
    }
}
