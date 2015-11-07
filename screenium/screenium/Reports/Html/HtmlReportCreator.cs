//original license: MIT
//
//See the file license.txt for copying permission.

using screenium.Reports.Html;
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
        private HtmlSupport _htmlSupport = new HtmlSupport();

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

            var sideBySideCreator = new HtmlSideBySideReportCreator(_argProc);

            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    //TODO main Suite report: use a template.html file for report!
                    sw.Write(_htmlSupport.GetTagStart("html"));
                    sw.Write(GetHeader("screenium Test Suite Results"));
                    sw.Write(_htmlSupport.GetTagStart("body"));
                    WriteReportHeadingHtml(sw, reports, reports.SuiteName);
                    sw.Write(_htmlSupport.GetSeparator());

                    foreach (var report in reports.Reports)
                    {
                        WriteReportHtml(dirManager, sw, report, null);
                        sw.Write(_htmlSupport.GetSeparator());

                        using (var memoryStream = new MemoryStream())
                        {
                            using (var writerToMemory = new StreamWriter(memoryStream))
                            {
                                CreateReportHeadingHtml(writerToMemory, report, reports.SuiteName);
                                writerToMemory.Flush();
                                sideBySideCreator.CreateSideBySideFiles(report.Test, GetStringFromStream(memoryStream));
                            }
                        }
                    }
                    sw.Write(_htmlSupport.GetTagEnd("body"));
                    sw.Write(_htmlSupport.GetTagEnd("html"));
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
            sw.Write(_htmlSupport.GetTagStart("table"));
            if (!string.IsNullOrWhiteSpace(suiteName))
            {
                WriteHtmlRow(sw, "Test Suite:", suiteName);
            }
            WriteHtmlRow(sw, "Test:", _htmlSupport.GetLink(report.Test.Name, report.Test.Url));
            WriteHtmlRow(sw, "Result: ", GetResultAsHtml(report.Result.Result));
            WriteHtmlRow(sw, "Tolerance: ", report.Result.Tolerance);
            WriteHtmlRow(sw, "Distortion: ", report.Result.Distortion);
            sw.Write(_htmlSupport.GetTagEnd("table"));
        }

        private void WriteReportHtml(DirectoryManager dirManager, StreamWriter sw, Report report, string suiteName)
        {
            CreateReportHeadingHtml(sw, report, suiteName);

            sw.Write(_htmlSupport.GetTagStart("table"));
            WriteHtmlRow(sw, "Diff Image: ", CreateImageHtmlWithLinkToSideBySide(report.Test, dirManager));
            sw.Write(_htmlSupport.GetTagEnd("table"));
        }

        private string CreateImageHtmlWithLinkToSideBySide(TestDescription test, DirectoryManager dirManager)
        {
            CopyImagesToReportOutputDir(test, dirManager);

            string html = "";

            var diffImageFileName = dirManager.GetDiffImageFileName(test.Name);
            var imageHtml = _htmlSupport.CreateImageHtml(diffImageFileName, "diff image");
            html += _htmlSupport.GetLink(imageHtml, dirManager.GetSideBySideFilename(test));

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

        private void WriteReportHeadingHtml(StreamWriter sw, ReportSet reports, string suiteName)
        {
            sw.Write(_htmlSupport.GetTagStart("table"));

            WriteHtmlRow(sw, _htmlSupport.GetEmphasisedText("screenium Test Suite Results:"), "");
            WriteHtmlRow(sw, "Test Suite:", suiteName);
            WriteHtmlRow(sw, "Created: ", DateSupport.ToString(reports.Created));
            WriteHtmlRow(sw, "Duration: ", DateSupport.ToString(reports.Duration));
            WriteHtmlRow(sw, "Suite Result: ", _htmlSupport.GetHtmlColoredForResult(reports.OverallResult, reports.OverallResult.ToString()));

            var resultHtml = reports.CountTestsPassed + " of " + reports.CountTests + " tests passed.";
            WriteHtmlRow(sw, "Tests Passed: ", resultHtml);

            sw.Write(_htmlSupport.GetTagEnd("table"));
        }

        private string GetResultAsHtml(Compare.CompareResult compareResult)
        {
            return _htmlSupport.GetHtmlColoredForResult(compareResult, compareResult.ToString());
        }


        private string GetHeader(string title)
        {
            return _htmlSupport.GetTagStart("head") +
                _htmlSupport.GetTagWithChildText("title", title) +
                _htmlSupport.GetTagEnd("head");
        }

        private void WriteHtmlRow(StreamWriter sw, string name, string value)
        {
            sw.Write(_htmlSupport.GetTagStart("tr"));
            sw.Write(_htmlSupport.GetTagStart("td"));
            sw.Write(name);
            sw.Write(_htmlSupport.GetTagEnd("td"));
            sw.Write(_htmlSupport.GetTagStart("td"));
            sw.Write(value);
            sw.Write(_htmlSupport.GetTagEnd("td"));
            sw.Write(_htmlSupport.GetTagEnd("tr"));
        }

        private void WriteHtmlRow(StreamWriter sw, string name, double value)
        {
            WriteHtmlRow(sw, name, value.ToString());
        }

        public void ShowReport(ReportSet reports)
        {
            Reports.WindowsSupport.OpenFileInExplorer(reports.FilePath);
        }
    }
}
