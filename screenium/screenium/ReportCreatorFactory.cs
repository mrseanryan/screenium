//Copyright (c) 2015 Sean Ryan
//
//See the file license.txt for copying permission.

namespace screenium
{
    class ReportCreatorFactory
    {
        internal static IReportCreator Create()
        {
            return new ConsoleReportCreator();
        }
    }
}
