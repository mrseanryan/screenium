//original license: MIT
//
//See the file license.txt for copying permission.

using System.IO;

namespace screenium
{
    internal class DirectoryManager
    {
        private string imagesDirPath;
        private string extension = "PNG";
        private ArgsProcessor _argProc;

        public DirectoryManager(ArgsProcessor argProc)
        {
            _argProc = argProc;

            imagesDirPath = argProc.GetArg(ArgsProcessor.Args.IMAGES_DIR_PATH);
        }

        internal string GetDiffImageFilePath(string testName)
        {
            var filename = GetDiffImageFileName(testName);

            return Path.GetFullPath(Path.Combine(imagesDirPath, filename));
        }

        internal string GetDiffImageFileName(string testName)
        {
            string filename = CreateFilenameFromTest(testName + "__diff");
            return filename;
        }

        internal string GetExpectedImageFilePath(TestDescription test)
        {
            string filename = GetExpectedImageFilename(test);

            return Path.GetFullPath(Path.Combine(imagesDirPath, filename));
        }

        internal string GetExpectedImageFilename(TestDescription test)
        {
            string filename = CreateFilenameFromTest(test.Name + "__expected");
            return filename;
        }

        private string CreateFilenameFromTest(string testName, bool isWithExtension = true)
        {
            string illegals = " ;/\\$.,<>";
            string filename = testName;
            foreach (char illegal in illegals)
            {
                filename = filename.Replace(illegal, '_');
            }
            if (isWithExtension)
            {
                filename += "." + extension;
            }
            return filename;
        }

        private string CreateFilenameFromTestNoExtension(string testName)
        {
            return CreateFilenameFromTest(testName, false);
        }

        internal string GetActualImageFilePath(string testName)
        {
            var filePath = Path.GetFullPath(Path.Combine(imagesDirPath, GetActualImageFilename(testName)));
            return filePath;
        }

        internal string GetActualImageFilePath(TestDescription test)
        {
            return GetActualImageFilePath(test.Name);
        }

        internal string GetActualImageFilename(string testName)
        {
            var filename = CreateFilenameFromTest(testName + "__actual");
            return filename;
        }

        internal string GetSideBySideFilename(TestDescription test)
        {
            string filename = CreateFilenameFromTestNoExtension(test.Name) + ".sideBySide.html";
            return filename;
        }

        internal string GetOutputDirectoryFullPath()
        {
            return Path.GetDirectoryName(_argProc.GetArg(ArgsProcessor.Args.OUTPUT_FILE_PATH));
        }
    }
}
