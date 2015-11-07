//original license: MIT
//
//See the file license.txt for copying permission.

using screenium.Compare;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml;
using System.Xml.Serialization;

namespace screenium.Reports
{
    /// <summary>
    /// public class so XML serializer can use it.
    /// </summary>
    public class CombinedReportData
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

        TimeSpan _duration;
        [XmlIgnore]
        public TimeSpan Duration
        {
            get
            {
                return _duration;
            }

            set
            {
                DurationHumanReadableInXml = DateSupport.ToString(value);
                _duration = value;
            }
        }

        public string DurationHumanReadableInXml { get; set; }

        // XmlSerializer does not support TimeSpan, so use this property for 
        // serialization instead.
        [Browsable(false)]
        [XmlElement(DataType = "duration", ElementName = "DurationXml")]
        public string TimeSinceLastEventString
        {
            get
            {
                return XmlConvert.ToString(Duration);
            }
            set
            {
                Duration = string.IsNullOrWhiteSpace(value) ?
                    TimeSpan.Zero : XmlConvert.ToTimeSpan(value);
            }
        }

        public List<CompareResult> OverallResults { get; set; }

        public int TotalTests { get; set; }
        public int TotalPassed { get; set; }

        public List<ReportSet> ReportSets { get; set; }

        public DateTime Updated { get; set; }
    }
}
