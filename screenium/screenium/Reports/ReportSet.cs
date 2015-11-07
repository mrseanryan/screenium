//original license: MIT
//
//See the file license.txt for copying permission.

using System;
using System.Collections.Generic;
using System.Linq;

namespace screenium.Reports
{
    public class ReportSet
    {
        public ReportSet()
        {
            Reports = new List<Report>();
        }

        public List<Report> Reports { get; set; }

        public string FilePath { get; set; }

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

        /// <summary>
        /// convenience method to make code clearer.
        /// </summary>
        public string SuiteName
        {
            get { return CsvFileName; }
        }
        public Compare.CompareResult OverallResult { get; set; }
        public string Exception { get; set; }
    }
}
