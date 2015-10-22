//Copyright (c) 2015 Sean Ryan
//
//See the file license.txt for copying permission.

namespace screenium
{
    interface IReportCreator
    {
        Report CreateReport(TestDescription test, CompareResultDescription compareResult, string filePath);
        void ShowReport(Report report);
    }
}
