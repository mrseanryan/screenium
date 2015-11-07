//original license: MIT
//
//See the file license.txt for copying permission.

using System;
using System.ComponentModel;
using System.Xml;
using System.Xml.Serialization;

namespace screenium.Reports
{
    /// <summary>
    /// horrible code to be able to serialize TimeSpan to XML via XmlSerializer
    /// </summary>
    /// <remarks>
    /// any way to do mixins with C#?
    /// OR else move away from XmlSerializer!</remarks>
    public class XmlDurationHolder
    {
        //TODO replace this horrible code with other solution -> move away from XmlSerializer ?

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
    }
}