//original license: MIT
//
//See the file license.txt for copying permission.

using System.Collections.Generic;
using System.IO;

namespace screenium
{
    class ActualToExpectedConverter
    {
        private ArgsProcessor _argProc;

        public ActualToExpectedConverter(ArgsProcessor argProc)
        {
            this._argProc = argProc;
        }

        internal void Convert(List<TestDescription> tests)
        {
            var dirManager = new DirectoryManager(_argProc);
            
            foreach (var test in tests)
            {
                Outputter.Output("Accepting latest changes for test: " + test.Name);

                var actualPath = dirManager.GetActualImageFilePath(test);
                var expectedPath = dirManager.GetExpectedImageFilePath(test);
                File.Copy(actualPath, expectedPath, true);
            }
            Outputter.Output("Accepted latest changes for " + tests.Count + " tests.");
        }
    }
}
