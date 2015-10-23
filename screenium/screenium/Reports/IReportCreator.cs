//Copyright (c) 2015 Sean Ryan
//
//See the file license.txt for copying permission.

namespace screenium
{
    interface IReportCreator
    {
        bool HasSaveCapability();

        void SaveReport(Report report, ArgsProcessor argProc);

        void ShowReport(Report report);
    }
}
