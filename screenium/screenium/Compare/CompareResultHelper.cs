//Copyright (c) 2015 Sean Ryan
//
//See the file license.txt for copying permission.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace screenium.Compare
{
    class CompareResultHelper
    {
        internal static ConsoleColor GetResultAsConsoleColor(CompareResult result)
        {
            switch (result)
            {
                case Compare.CompareResult.Similar:
                    return ConsoleColor.Green;
                case Compare.CompareResult.Different:
                    return ConsoleColor.Red;
                default:
                    throw new ArgumentException("Not a recognised CompareResult: " + result);
            }
        }
        
        internal static string GetResultAsString(CompareResult result)
        {
            switch (result)
            {
                case CompareResult.Similar:
                    return "OK";
                case CompareResult.Different:
                    return "Differences found";
                case CompareResult.Cancelled:
                    return "Cancelled";
                default:
                    throw new ArgumentException("not a recognised CompareResult: " + result);
            }
        }
    }
}
