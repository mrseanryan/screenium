//Copyright (c) 2015 Sean Ryan
//
//See the file license.txt for copying permission.

using System;

namespace screenium
{
    class Outputter
    {
        internal static void Output(string text)
        {
            Console.WriteLine(text);
        }

        internal static void Output(Exception ex)
        {
            Output(ex.Message);
            Output(ex.StackTrace);

            if (ex.InnerException != null)
            {
                Output(ex.InnerException);
            }
        }
    }
}
