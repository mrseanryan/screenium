//Copyright (c) 2015 Sean Ryan
//
//See the file license.txt for copying permission.

namespace screenium
{
    interface IReportCreator
    {
        Report CreateReport(TestDescription test, CompareResult compareResult, string filePath);
        void ShowReport(Report report);
    }
}
