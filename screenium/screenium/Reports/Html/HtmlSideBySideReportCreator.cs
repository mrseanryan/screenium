//original license: MIT
//
//See the file license.txt for copying permission.

using System.IO;

namespace screenium.Reports.Html
{
    class HtmlSideBySideReportCreator
    {
        private ArgsProcessor _argProc;

        public HtmlSideBySideReportCreator(ArgsProcessor argProc)
        {
            _argProc = argProc;
        }

        internal void CreateSideBySideFiles(TestDescription test, string testHeaderHtml)
        {
            var dirManager = new DirectoryManager(_argProc);

            var _templateCreator = new TemplateCreator(_argProc, TemplateCreator.TemplateNameSideBySide);

            _templateCreator.SetTemplateParam(TemplateCreator.TemplateParamHeader, testHeaderHtml);
            _templateCreator.SetTemplateParam(TemplateCreator.TemplateParamExpectedImage, dirManager.GetExpectedImageFilename(test));
            _templateCreator.SetTemplateParam(TemplateCreator.TemplateParamActualImage, dirManager.GetActualImageFilename(test.Name));
            _templateCreator.SetTemplateParam(TemplateCreator.TemplateParamName, test.Name);

            string outFilename = dirManager.GetSideBySideFilename(test);
            string outFilePath = Path.Combine(dirManager.GetOutputDirectoryFullPath(), outFilename);
            _templateCreator.Save(outFilePath);
        }
    }
}
