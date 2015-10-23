//Copyright (c) 2015 Sean Ryan
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

        void IReportCreator.SaveReport(Report report, ArgsProcessor argProc)
        {
            throw new NotSupportedException();
        }

        void IReportCreator.ShowReport(Report report)
        {
            Outputter.OutputEmphasised("Test Results:");

            Outputter.Output("Test: " + report.Test.Name);
            Outputter.Output("Result: " + report.Result.Result, GetColorForResult(report.Result.Result));
            Outputter.Output("Tolerance: " + report.Result.Tolerance);
            Outputter.Output("Distortion: " + report.Result.Distortion);
        }

        private ConsoleColor GetColorForResult(Compare.CompareResult compareResult)
        {
            switch (compareResult)
            {
                case Compare.CompareResult.Similar:
                    return ConsoleColor.Green;
                case Compare.CompareResult.Different:
                    return ConsoleColor.Red;
                default:
                    throw new ArgumentException("Not a recognised CompareResult: " + compareResult);
            }
        }
    }
}
