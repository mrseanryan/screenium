//Copyright (c) 2015 Sean Ryan
//
//See the file license.txt for copying permission.

using System.Collections.Generic;

namespace screenium.Reports
{
    class ReportSet
    {
        internal ReportSet()
        {
            Reports = new List<Report>();
        }

        internal List<Report> Reports { get; set; }

        internal string FilePath { get; set; }
    }
}
