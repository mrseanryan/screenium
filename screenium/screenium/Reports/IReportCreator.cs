//Copyright (c) 2015 Sean Ryan
//
//See the file license.txt for copying permission.

namespace screenium.Reports
{
    interface IReportCreator
    {
        bool HasSaveCapability();

        void SaveReport(ReportSet report, ArgsProcessor argProc);

        void ShowReport(ReportSet report);
    }
}
