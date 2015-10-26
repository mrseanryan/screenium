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

        internal static void Output(string text, ConsoleColor color)
        {
            var orig = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ForegroundColor = orig;
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

        internal static void OutputEmphasised(string text)
        {
            Output(text);
            Output(new string('=', text.Length));
        }

        internal static void OutputSeparator()
        {
            Output("----------");
        }
    }
}
