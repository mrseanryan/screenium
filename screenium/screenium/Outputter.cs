//original license: MIT
//
//See the file license.txt for copying permission.

using System;

namespace screenium
{
    class Outputter
    {
        private static ConsoleColor _origColor = Console.ForegroundColor;

        internal static void Output(string text)
        {
            Console.WriteLine(text);
        }

        internal static void Output(string text, ConsoleColor color)
        {
            SetConsoleColor(color);
            Console.WriteLine(text);
            RestoreConsoleColor();
        }

        private static void SetConsoleColor(ConsoleColor color)
        {
            Console.ForegroundColor = color;
        }

        private static void RestoreConsoleColor()
        {
            Console.ForegroundColor = _origColor;
        }

        internal static void Output(Exception ex)
        {
            Output(ex.Message, ConsoleColor.Red);
            Output(ex.StackTrace);

            if (ex.InnerException != null)
            {
                Output(ex.InnerException);
            }
        }

        internal static void OutputEmphasised(string text, ConsoleColor color)
        {
            SetConsoleColor(color);
            OutputEmphasised(text);
            RestoreConsoleColor();
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
