//original license: MIT
//
//See the file license.txt for copying permission.

using screenium.Compare;

namespace screenium.Reports
{
    public class Report
    {
        public CompareResultDescription Result { get; set; }

        public TestDescription Test { get; set; }
    }
}
