//original license: MIT
//
//See the file license.txt for copying permission.

using System.IO;
namespace screenium.Reports
{
    class TemplateCreator
    {
        private ArgsProcessor _argProc;

        private const string _templateNameSideBySide = "sideXside.template.html";

        private const string _templateParamHeader = "{TEST_HEADER}";
        private const string _templateParamExpectedImage = "{PATH_TO_EXPECTED_IMAGE}";
        private const string _templateParamActualImage = "{PATH_TO_ACTUAL_IMAGE}";
        private const string _templateParamName = "{TEST_NAME}";

        public TemplateCreator(ArgsProcessor argProc)
        {
            this._argProc = argProc;
        }

        internal void CreateSideBySideFiles(TestDescription test, string testHeaderHtml)
        {
            var dirManager = new DirectoryManager(_argProc);

            var templateHtml = ReadHtmlFromTemplate(_templateNameSideBySide);

            SetTemplateParam(_templateParamHeader, testHeaderHtml, ref templateHtml);
            SetTemplateParam(_templateParamExpectedImage, dirManager.GetExpectedImageFilename(test), ref templateHtml);
            SetTemplateParam(_templateParamActualImage, dirManager.GetActualImageFilename(test.Name), ref templateHtml);
            SetTemplateParam(_templateParamName, test.Name, ref templateHtml);

            string outFilename = dirManager.GetSideBySideFilename(test);
            string outFilePath = Path.Combine(dirManager.GetOutputDirectoryFullPath(), outFilename);
            SaveHtml(templateHtml, outFilePath);
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
