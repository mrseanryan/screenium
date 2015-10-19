
using System;
namespace screenium
{
    class Program
    {
        static int Main(string[] args)
        {
            try
            {
                ArgsProcessor argProc = new ArgsProcessor(args);

                if (!argProc.Validate())
                {
                    Console.WriteLine(argProc.GetUsage());
                    return 1;
                }

                if (argProc.IsOptionOn(ArgsProcessor.Options.TestSelf))
                {
                    Log("Running self test ...");
                    BrowserDriver driver = new BrowserDriver();
                    driver.TestChrome();
                }

                throw new NotImplementedException();
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.StackTrace + "\n" + e.Message);
                return 2;
            }

            return 0;
        }

        private static void Log(string text)
        {
            Console.WriteLine(text);
        }
    }
}
