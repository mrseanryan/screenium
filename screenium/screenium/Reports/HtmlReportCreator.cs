//original license: MIT
//
//See the file license.txt for copying permission.


using System;
using System.IO;

namespace screenium.Reports
{
    /// <summary>
    /// a HTML report creator
    /// it can be just 1 html page, with the diff image for each test run.
    /// we open it via Windows Explorer.
    /// </summary>
    class HtmlReportCreator : IReportCreator
    {
        public bool HasSaveCapability()
        {
            return true;
        }

        public void SaveReport(ReportSet reports, ArgsProcessor argProc)
        {
            string filePath = argProc.GetArg(ArgsProcessor.Args.OUTPUT_FILE_PATH);
            string extension = "html";
            if (string.Compare(Path.GetExtension(filePath), "." + extension, StringComparison.OrdinalIgnoreCase) != 0)
            {
                throw new InvalidOperationException("Report output filename must end with ." + extension);
            }

            DirectoryManager dirManager = new DirectoryManager(argProc);

            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write(GetTagStart("html"));
                    sw.Write(GetHeader("screenium Test Results"));
                    sw.Write(GetTagStart("body"));
                    WriteReportHeadingHtml(sw);
                    sw.Write(GetSeparator());

                    foreach (var report in reports.Reports)
                    {
                        WriteReportHtml(dirManager, sw, report);
                        sw.Write(GetSeparator());
                    }
                    sw.Write(GetTagEnd("body"));
                    sw.Write(GetTagEnd("html"));
                    sw.Flush();
                }
            }

            reports.FilePath = filePath;
        }

        private void WriteReportHtml(DirectoryManager dirManager, StreamWriter sw, Report report)
        {
            sw.Write(GetTagStart("table"));
            WriteHtmlRow(sw, "Test:", GetLink(report.Test.Name, report.Test.Url));
            WriteHtmlRow(sw, "Result: ", GetResultAsHtml(report.Result.Result));
            WriteHtmlRow(sw, "Tolerance: ", report.Result.Tolerance);
            WriteHtmlRow(sw, "Distortion: ", report.Result.Distortion);

            var diffImageFilePath = dirManager.GetDiffImageFilePath(report.Test.Name);
            WriteHtmlRow(sw, "Diff Image: ", CreateImageHtml(diffImageFilePath, "diff image"));

            sw.Write(GetTagEnd("table"));
        }

        private string GetLink(string text, string url)
        {
            return GetTagStartWithAttributes("a", "href='" + url + "' target='_new_window'") + text + GetTagEnd("a");
        }

        private void WriteReportHeadingHtml(StreamWriter sw)
        {
            sw.Write(GetTagStart("table"));
            sw.Write(GetTagStart("tr"));
            sw.Write(GetTagStart("td"));
            sw.Write(GetEmphasisedText("screenium Test Results:"));
            sw.Write(GetTagEnd("td"));
            sw.Write(GetTagEnd("tr"));
            sw.Write(GetTagEnd("table"));
        }

        private string GetSeparator()
        {
            return GetTagStart("hr") + GetTagEnd("hr");
        }

        private string GetResultAsHtml(Compare.CompareResult compareResult)
        {
            string color;
            const string green = "#00FF00";
            const string red = "#FF0000";
            switch (compareResult)
            {
                case Compare.CompareResult.Similar:
                    color = green;
                    break;
                case Compare.CompareResult.Different:
                    color = red;
                    break;
                default:
                    throw new ArgumentException("Not a recognised Report Result: " + compareResult);
            }
            return GetTagWithAttributesAndChildText("div", "style='background-color:" + color + "'", compareResult.ToString());
        }

        private string GetHeader(string title)
        {
            return GetTagStart("head") +
                GetTagWithChildText("title", title) +
                GetTagEnd("head");
        }

        private string GetTagWithAttributesAndChildText(string tag, string attributes, string text)
        {
            return GetTagStartWithAttributes(tag, attributes) + text + GetTagEnd(tag);
        }

        private string GetTagWithChildText(string tag, string text)
        {
            return GetTagStart(tag) + text + GetTagEnd(tag);
        }

        private string GetEmphasisedText(string text)
        {
            return GetTagStart("b") + text + GetTagEnd("b");
        }

        private string CreateImageHtml(string imageFilePath, string altText)
        {
            return "<img src='" + imageFilePath + "' alt='"+altText+"' />";
        }

        private void WriteHtmlRow(StreamWriter sw, string name, string value)
        {
            sw.Write(GetTagStart("tr"));
            sw.Write(GetTagStart("td"));
            sw.Write(name);
            sw.Write(GetTagEnd("td"));
            sw.Write(GetTagStart("td"));
            sw.Write(value);
            sw.Write(GetTagEnd("td"));
            sw.Write(GetTagEnd("tr"));
        }

        private void WriteHtmlRow(StreamWriter sw, string name, double value)
        {
            WriteHtmlRow(sw, name, value.ToString());
        }

        private string GetTagStart(string text)
        {
            return GetTagStartWithAttributes(text, "");
        }

        private string GetTagStartWithAttributes(string text, string attributes)
        {
            return "<" + text + " " + attributes + ">" + Environment.NewLine;
        }

        private string GetTagEnd(string text)
        {
            return "</" + text + ">" + Environment.NewLine;
        }

        public void ShowReport(ReportSet reports)
        {
            Reports.WindowsSupport.OpenFileInExplorer(reports.FilePath);
        }
    }
}
