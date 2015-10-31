//original license: MIT
//
//See the file license.txt for copying permission.

namespace screenium.Reports
{
    interface IReportCreator
    {
        bool HasSaveCapability();

        void SaveReport(ReportSet report);

        void ShowReport(ReportSet report);
    }
}
