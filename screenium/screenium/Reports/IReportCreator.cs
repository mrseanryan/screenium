//Copyright (c) 2015 Sean Ryan
//
//See the file license.txt for copying permission.

namespace screenium
{
    interface IReportCreator
    {
        Report CreateReport(TestDescription test, CompareResultDescription compareResult);

        bool HasSaveCapability();

        void SaveReport(Report report, string filePath);

        void ShowReport(Report report);
    }
}
