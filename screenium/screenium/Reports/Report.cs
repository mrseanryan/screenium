//original license: MIT
//
//See the file license.txt for copying permission.

using screenium.Compare;

namespace screenium.Reports
{
    class Report
    {
        internal CompareResultDescription Result { get; set; }

        internal TestDescription Test { get; set; }
    }
}
