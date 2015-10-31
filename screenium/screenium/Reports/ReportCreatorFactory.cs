//original license: MIT
//
//See the file license.txt for copying permission.

using System.Collections.Generic;

namespace screenium.Reports
{
    class ReportCreatorFactory
    {
        internal static List<IReportCreator> CreateReporters(ArgsProcessor argProc)
        {
            var reporters = new List<IReportCreator> {new ConsoleReportCreator(), new HtmlReportCreator(argProc)};

            return reporters;
        }
    }
}
