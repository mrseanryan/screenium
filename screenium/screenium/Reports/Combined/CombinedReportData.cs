//original license: MIT
//
//See the file license.txt for copying permission.

using screenium.Compare;
using System;
using System.Collections.Generic;

namespace screenium.Reports
{
    /// <summary>
    /// public class so XML serializer can use it.
    /// </summary>
    public class CombinedReportData : XmlDurationHolder
    {
        public CombinedReportData()
        {
            OverallResults = new List<CompareResult>(0);
            ReportSets = new List<ReportSet>(0);
        }

        public DateTime StartTime
        {
            get;
            set;
        }

        public List<CompareResult> OverallResults { get; set; }

        public int TotalTests { get; set; }
        public int TotalPassed { get; set; }

        public List<ReportSet> ReportSets { get; set; }

        public DateTime Updated { get; set; }
    }
}
