//original license: MIT
//
//See the file license.txt for copying permission.

using System.Collections.Generic;

namespace screenium.Reports
{
    class ReportCreatorFactory
    {
        internal static List<IReportCreator> CreateReporters()
        {
            var reporters = new List<IReportCreator> {new ConsoleReportCreator(), new HtmlReportCreator()};

            return reporters;
        }
    }
}
