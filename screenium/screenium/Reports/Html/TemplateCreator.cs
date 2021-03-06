﻿//original license: MIT
//
//See the file license.txt for copying permission.

using System;
using System.IO;
namespace screenium.Reports
{
    class TemplateCreator
    {
        private ArgsProcessor _argProc;

        internal const string TemplateNameSideBySide = "sideXside.template.html";
        internal const string TemplateNameCombinedReport = "combinedReport.template.html";
        internal const string TemplateNameCombinedReportTestPart = "combinedReport.testPart.template.html";

        //TODO template: use enums not strings!

        //the combined report:
        internal const string TemplateParamCombinedStartTime = "{COMBINED_START_TIME}";
        internal const string TemplateParamCombinedDuration = "{COMBINED_DURATION}";
        internal const string TemplateParamCombinedResult = "{COMBINED_RESULT}";
        internal const string TemplateParamCombinedTestsPassed = "{COMBINED_TESTS_PASSED}";
        internal const string TemplateParamCombinedTestsTotal = "{COMBINED_TESTS_TOTAL}";
        internal const string TemplateParamCombinedSuiteParts = "{COMBINED_TEST_SUITES_PARTS}";

        //the test-suite part of the combined report:
        internal const string TemplateParamSuiteName = "{TEST_SUITE_NAME}";
        internal const string TemplateParamSuiteResult = "{TEST_SUITE_RESULT}";
        internal const string TemplateParamSuiteTestsPassed = "{TEST_SUITE_TESTS_PASSED}";
        internal const string TemplateParamSuiteTestsTotal = "{TEST_SUITE_TESTS_TOTAL}";
        internal const string TemplateParamSuiteReportLink = "{TEST_SUITE_REPORT_LINK}";

        //side by site report:
        internal const string TemplateParamHeader = "{TEST_HEADER}";
        internal const string TemplateParamExpectedImage = "{PATH_TO_EXPECTED_IMAGE}";
        internal const string TemplateParamActualImage = "{PATH_TO_ACTUAL_IMAGE}";

        internal const string TemplateParamName = "{TEST_NAME}";

        private string _templateHtml;

        public TemplateCreator(ArgsProcessor argProc, string templateName)
        {
            this._argProc = argProc;
            _templateHtml = ReadHtmlFromTemplate(templateName);
        }

        internal void SetTemplateParam(string templateParam, string value)
        {
            SetTemplateParam(templateParam, value, ref _templateHtml);
        }
        internal void SetTemplateParam(string templateParam, int value)
        {
            SetTemplateParam(templateParam, value.ToString());
        }

        internal void SetTemplateParam(string templateParam, DateTime value)
        {
            SetTemplateParam(templateParam, DateSupport.ToString(value));
        }

        internal void SetTemplateParam(string templateParam, TimeSpan value)
        {
            SetTemplateParam(templateParam, DateSupport.ToString(value));
        }

        internal void Save(string outFilePath)
        {
            SaveHtml(_templateHtml, outFilePath);
        }

        private void SaveHtml(string templateHtml, string filePath)
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write(templateHtml);
                    sw.Flush();
                }
            }
        }

        public override string ToString()
        {
            return _templateHtml;
        }

        private void SetTemplateParam(string paramName, string value, ref string templateHtml)
        {
            templateHtml = templateHtml.Replace(paramName, value);
        }

        private string ReadHtmlFromTemplate(string templateName)
        {
            var filePath = Path.GetFullPath(
                Path.Combine(_argProc.GetArg(ArgsProcessor.Args.TEMPLATES_DIR_PATH), templateName));

            using (FileStream fs = new FileStream(filePath, FileMode.Open))
            {
                using (TextReader sw = new StreamReader(fs))
                {
                    return sw.ReadToEnd();
                }
            }
        }
    }
}
