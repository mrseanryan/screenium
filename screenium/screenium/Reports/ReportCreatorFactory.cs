//Copyright (c) 2015 Sean Ryan
//
//See the file license.txt for copying permission.

using System.Collections.Generic;
namespace screenium
{
    class ReportCreatorFactory
    {
        internal static List<IReportCreator> CreateReporters()
        {
            var reporters = new List<IReportCreator>();

            reporters.Add(new ConsoleReportCreator());
            reporters.Add(new HtmlReportCreator());

            return reporters;
        }
    }
}
