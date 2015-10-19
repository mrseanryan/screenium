using System;

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
            throw new NotImplementedException();
        }

        internal string GetTempFileName(TestDescription test)
        {
            throw new NotImplementedException();
        }
    }
}
