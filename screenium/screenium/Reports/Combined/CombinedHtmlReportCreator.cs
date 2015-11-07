//original license: MIT
//
//See the file license.txt for copying permission.

namespace screenium.Reports
{
    /// <summary>
    /// creates/appends a combined summary that we append to at each run.
    /// </summary>
    class CombinedHtmlReportCreator
    {
        private CombinedReportPersistance _combinedPersist;

        internal void CreateReport(ArgsProcessor argProc)
        {
            _combinedPersist = new CombinedReportPersistance(new DirectoryManager(argProc));

            var combined = _combinedPersist.LoadCombinedData();



            //TODO output to combined report HTML
        }
    }
}
