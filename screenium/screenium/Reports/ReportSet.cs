//original license: MIT
//
//See the file license.txt for copying permission.

using System;
using System.Collections.Generic;
using System.Linq;

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

        public TimeSpan Duration { get; set; }

        public DateTime Created { get; set; }

        public int CountTests
        {
            get
            {
                return Reports.Count();
            }
        }

        public int CountTestsPassed
        {
            get
            {
                return Reports.Count(report => report.Result.Result == Compare.CompareResult.Similar);
            }
        }

        public string CsvFileName { get; set; }

        public Compare.CompareResult OverallResult { get; set; }
    }
}
