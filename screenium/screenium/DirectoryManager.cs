//Copyright (c) 2015 Sean Ryan
//
//See the file license.txt for copying permission.

using System;
using System.IO;

namespace screenium
{
    internal class DirectoryManager
    {
        private string imagesDirPath;
        private string extension = "PNG";

        public DirectoryManager(ArgsProcessor argProc)
        {
            imagesDirPath = argProc.GetArg(ArgsProcessor.Args.IMAGES_DIR_PATH);
        }

        internal string GetDiffImageFilePath(string testName)
        {
            string filename = CreateFilenameFromTest(testName + "__diff");

            return Path.GetFullPath(Path.Combine(imagesDirPath, filename));
        }

        internal string GetExpectedImageFilePath(TestDescription test)
        {
            string filename = CreateFilenameFromTest(test.Name);

            return Path.GetFullPath(Path.Combine(imagesDirPath, filename));
        }

        private string CreateFilenameFromTest(string testName)
        {
            string illegals = " ;/\\$.,<>";
            string filename = testName;
            foreach (char illegal in illegals)
            {
                filename = filename.Replace(illegal, '_');
            }
            return filename + "." + extension;
        }

        internal string GetActualImageFilePath(string testName)
        {
            var filePath = Path.GetFullPath(Path.Combine(imagesDirPath, CreateFilenameFromTest(testName) + "__actual." + extension));
            return filePath;
        }

        internal string GetActualImageFilePath(TestDescription test)
        {
            return GetActualImageFilePath(test.Name);
        }
    }
}
