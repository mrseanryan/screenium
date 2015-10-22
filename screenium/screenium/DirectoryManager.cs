//Copyright (c) 2015 Sean Ryan
//
//See the file license.txt for copying permission.

using System;
using System.IO;

namespace screenium
{
    class DirectoryManager
    {
        private string imagesDirPath;
        
        public DirectoryManager(ArgsProcessor argProc)
        {
            imagesDirPath = argProc.GetArg(ArgsProcessor.Args.IMAGES_DIR_PATH);
        }

        internal string GetExpectedImageFilePath(TestDescription test)
        {
            string filename = CreateFilenameFromTest(test.Name, "PNG");

            return Path.Combine(imagesDirPath, filename);
        }

        private string CreateFilenameFromTest(string testName, string extension)
        {
            string illegals = " ;/\\$.,<>";
            string filename = testName;
            foreach (char illegal in illegals)
            {
                filename = filename.Replace(illegal, '_');
            }
            return filename + "." + extension;
        }

        internal string GetTempFilePath(TestDescription test, string extension)
        {
            var filePath = Path.Combine(Path.GetTempPath(), CreateFilenameFromTest(test.Name, "PNG"));
            return filePath;
        }
    }
}
