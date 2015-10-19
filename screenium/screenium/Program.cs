
using System;
using System.Collections.Generic;
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

                List<TestDescription> tests;
                {
                    var reader = new CsvReader();
                    var path = argProc.GetArg(ArgsProcessor.Args.CSV_FILE_PATH);
                    tests = reader.ReadFromFilePath(path);
                    Log(string.Format("Read {0} tests from CSV file {1}", tests.Count, path));
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
