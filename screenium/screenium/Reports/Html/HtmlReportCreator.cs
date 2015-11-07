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
        private ArgsProcessor _argProc;
        private int _targetId = 1;

        public HtmlReportCreator(ArgsProcessor argProc)
        {
            _argProc = argProc;
        }

        public bool HasSaveCapability()
        {
            return true;
        }

        public void SaveReport(ReportSet reports)
        {
            string filePath = _argProc.GetArg(ArgsProcessor.Args.OUTPUT_FILE_PATH);
            string extension = "html";
            if (string.Compare(Path.GetExtension(filePath), "." + extension, StringComparison.OrdinalIgnoreCase) != 0)
            {
                throw new InvalidOperationException("Report output filename must end with ." + extension);
            }

            DirectoryManager dirManager = new DirectoryManager(_argProc);

            var templateCreator = new TemplateCreator(_argProc);

            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write(GetTagStart("html"));
                    sw.Write(GetHeader("screenium Test Suite Results"));
                    sw.Write(GetTagStart("body"));
                    WriteReportHeadingHtml(sw, reports, reports.SuiteName);
                    sw.Write(GetSeparator());

                    foreach (var report in reports.Reports)
                    {
                        WriteReportHtml(dirManager, sw, report, null);
                        sw.Write(GetSeparator());

                        using (var memoryStream = new MemoryStream())
                        {
                            using (var writerToMemory = new StreamWriter(memoryStream))
                            {
                                CreateReportHeadingHtml(writerToMemory, report, reports.SuiteName);
                                writerToMemory.Flush();
                                templateCreator.CreateSideBySideFiles(report.Test, GetStringFromStream(memoryStream));
                            }
                        }
                    }
                    sw.Write(GetTagEnd("body"));
                    sw.Write(GetTagEnd("html"));
                    sw.Flush();
                }
            }

            reports.FilePath = filePath;
        }

        private string GetStringFromStream(MemoryStream memStream)
        {
            var sr = new StreamReader(memStream);
            {
                memStream.Position = 0;
                return sr.ReadToEnd();
            }
        }

        private void CreateReportHeadingHtml(StreamWriter sw, Report report, string suiteName)
        {
            sw.Write(GetTagStart("table"));
            if (!string.IsNullOrWhiteSpace(suiteName))
            {
                WriteHtmlRow(sw, "Test Suite:", suiteName);
            }
            WriteHtmlRow(sw, "Test:", GetLink(report.Test.Name, report.Test.Url));
            WriteHtmlRow(sw, "Result: ", GetResultAsHtml(report.Result.Result));
            WriteHtmlRow(sw, "Tolerance: ", report.Result.Tolerance);
            WriteHtmlRow(sw, "Distortion: ", report.Result.Distortion);
            sw.Write(GetTagEnd("table"));
        }

        private void WriteReportHtml(DirectoryManager dirManager, StreamWriter sw, Report report, string suiteName)
        {
            CreateReportHeadingHtml(sw, report, suiteName);

            sw.Write(GetTagStart("table"));
            WriteHtmlRow(sw, "Diff Image: ", CreateImageHtmlWithLinkToSideBySide(report.Test, dirManager));
            sw.Write(GetTagEnd("table"));
        }

        private string CreateImageHtmlWithLinkToSideBySide(TestDescription test, DirectoryManager dirManager)
        {
            CopyImagesToReportOutputDir(test, dirManager);

            string html = "";

            var diffImageFileName = dirManager.GetDiffImageFileName(test.Name);
            var imageHtml = CreateImageHtml(diffImageFileName, "diff image");
            html += GetLink(imageHtml, dirManager.GetSideBySideFilename(test));

            return html;
        }

        private void CopyImagesToReportOutputDir(TestDescription test, DirectoryManager dirManager)
        {
            //copy images to output dir, so that the report is self-contained:
            var expectedImageFilePath = dirManager.GetExpectedImageFilePath(test);
            var diffImageFilePath = dirManager.GetDiffImageFilePath(test.Name);
            var actualImageFilePath = dirManager.GetActualImageFilePath(test.Name);

            var outputDir = Path.GetDirectoryName(_argProc.GetArg(ArgsProcessor.Args.OUTPUT_FILE_PATH));
            var expectedImageFilePathInOutputDir = Path.Combine(outputDir, Path.GetFileName(expectedImageFilePath));
            var actualImageFilePathInOutputDir = Path.Combine(outputDir, Path.GetFileName(actualImageFilePath));
            var diffImageFilePathInOutputDir = Path.Combine(outputDir, Path.GetFileName(diffImageFilePath));

            CopyFile(expectedImageFilePath, expectedImageFilePathInOutputDir);
            CopyFile(actualImageFilePath, actualImageFilePathInOutputDir);
            CopyFile(diffImageFilePath, diffImageFilePathInOutputDir);
        }

        private void CopyFile(string sourcePath, string destPath)
        {
            File.Copy(sourcePath, destPath, true);
        }

        private string GetLink(string text, string url)
        {
            return GetTagStartWithAttributes("a", "href='" + url + "' target='" + GetNextTargetId() + "'") + text + GetTagEnd("a");
        }

        private string GetNextTargetId()
        {
            return "_screenium_window_" + _targetId++;
        }

        private void WriteReportHeadingHtml(StreamWriter sw, ReportSet reports, string suiteName)
        {
            sw.Write(GetTagStart("table"));

            WriteHtmlRow(sw, GetEmphasisedText("screenium Test Suite Results:"), "");
            WriteHtmlRow(sw, "Test Suite:", suiteName);
            WriteHtmlRow(sw, "Created: ", DateSupport.ToString(reports.Created));
            WriteHtmlRow(sw, "Duration: ", DateSupport.ToString(reports.Duration));
            WriteHtmlRow(sw, "Suite Result: ", GetHtmlColoredForResult(reports.OverallResult, reports.OverallResult.ToString()));

            var resultHtml = reports.CountTestsPassed + " of " + reports.CountTests + " tests passed.";
            WriteHtmlRow(sw, "Tests Passed: ", resultHtml);

            sw.Write(GetTagEnd("table"));
        }

        private string GetSeparator()
        {
            return GetTagStart("hr") + GetTagEnd("hr");
        }

        private string GetResultAsHtml(Compare.CompareResult compareResult)
        {
            return GetHtmlColoredForResult(compareResult, compareResult.ToString());
        }

        private string GetHtmlColoredForResult(Compare.CompareResult compareResult, string text)
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
            return GetTagWithAttributesAndChildText("div", "style='background-color:" + color + "'", text);
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
            return "<img src='" + imageFilePath + "' alt='" + altText + "' />";
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
