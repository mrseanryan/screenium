﻿//Copyright (c) 2015 Sean Ryan
//
//See the file license.txt for copying permission.

using screenium.Compare;

namespace screenium.Reports
{
    class Report
    {
        internal CompareResultDescription Result { get; set; }

        internal TestDescription Test { get; set; }

        internal string FilePath { get; set; }
    }
}
