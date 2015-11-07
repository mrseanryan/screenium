//original license: MIT
//
//See the file license.txt for copying permission.

using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace screenium.Reports
{
    class CombinedReportPersistance
    {
        DirectoryManager _dirManager;
        string _path;

        public CombinedReportPersistance(DirectoryManager dirManager)
        {
            _dirManager = dirManager;
            _path = _dirManager.GetCombinedReportXmlPath();
        }

        internal void AddReportSet(ReportSet reportSet)
        {
            CombinedReportData combinedData = LoadCombinedData();

            combinedData.ReportSets.Add(reportSet);

            SaveCombinedData(combinedData);
        }

        public CombinedReportData LoadCombinedData()
        {
            if (!File.Exists(_path))
            {
                return new CombinedReportData();
            }
            CombinedReportData data;
            XmlSerializer xs = new XmlSerializer(typeof(CombinedReportData));
            using (TextReader tr = new StreamReader(_path))
            {
                data = (CombinedReportData)xs.Deserialize(tr);
                tr.Close();
            }
            return data;
        }

        private void SaveCombinedData(CombinedReportData combinedData)
        {
            UpdateData(combinedData);

            XmlSerializer xs = new XmlSerializer(typeof(CombinedReportData));
            using (TextWriter tw = new StreamWriter(_path))
            {
                xs.Serialize(tw, combinedData);
                tw.Close();
            }
        }

        private void UpdateData(CombinedReportData combinedData)
        {
            combinedData.Updated = DateTime.Now;

            combinedData.StartTime = combinedData.ReportSets
                .Aggregate((a, b) => a.Created.Ticks <= b.Created.Ticks ? a : b)
                .Created;

            combinedData.Duration = combinedData.ReportSets
                .Aggregate((a, b) => new ReportSet { Duration = a.Duration.Add(b.Duration) })
                .Duration;

            combinedData.ReportSets.ForEach(set => combinedData.OverallResults.Add(set.OverallResult));
            combinedData.OverallResults = combinedData.OverallResults.Distinct().ToList();

            int testsPassed = 0;
            combinedData.ReportSets.ForEach(set => testsPassed += set.CountTestsPassed);
            combinedData.TotalPassed = testsPassed;

            int testsTotal = 0;
            combinedData.ReportSets.ForEach(set => testsTotal += set.CountTests);
            combinedData.TotalTests = testsTotal;
        }
    }
}
