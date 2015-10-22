//Copyright (c) 2015 Sean Ryan
//
//See the file license.txt for copying permission.

namespace screenium
{
    class ReportCreatorFactory
    {
        internal static IReportCreator Create()
        {
            //TODO make a HTML report creator (and run both of them)
            //it can be just 1 html page, with the diff image for each test run.
            //we can open it via explorer (ref WeeWebWatcher ?)

            return new ConsoleReportCreator();
        }
    }
}
