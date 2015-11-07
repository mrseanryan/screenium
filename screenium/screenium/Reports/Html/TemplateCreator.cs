//original license: MIT
//
//See the file license.txt for copying permission.

using System.IO;
namespace screenium.Reports
{
    class TemplateCreator
    {
        private ArgsProcessor _argProc;

        internal const string TemplateNameSideBySide = "sideXside.template.html";

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

        internal void SetTemplateParam(string _templateParam, string value)
        {
            SetTemplateParam(_templateParam, value, ref _templateHtml);
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
