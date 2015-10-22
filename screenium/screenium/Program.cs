//Copyright (c) 2015 Sean Ryan
//
//See the file license.txt for copying permission.

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
                    RunSelfTest();
                }
                else
                {
                    RunTests(argProc);
                }
            }
            catch (Exception ex)
            {
                Outputter.Output(ex);
                return 2;
            }

            return 0;
        }

        private static void RunTests(ArgsProcessor argProc)
        {
            List<TestDescription> testsToRun = ReadTests(argProc);

            var runner = new TestRunner();
            runner.RunTests(testsToRun, argProc);

            Log("Finished running tests [OK]");
        }

        private static void RunSelfTest()
        {
            Log("Running self test ...");
            using (BrowserDriver driver = new BrowserDriver())
            {
                driver.TestChrome();
            }
            Log("self test ran [OK]");
        }

        private static List<TestDescription> ReadTests(ArgsProcessor argProc)
        {
            List<TestDescription> testsToRun;
            List<TestDescription> tests; 
            {
                var reader = new CsvReader();
                var path = argProc.GetArg(ArgsProcessor.Args.CSV_FILE_PATH);
                tests = reader.ReadFromFilePath(path);
                Log(string.Format("Read {0} tests from CSV file {1}", tests.Count, path));
            }

            testsToRun = TestDescription.GetTestsByName(tests, argProc.GetArg(ArgsProcessor.Args.TEST_NAME)); 
            return testsToRun;
        }

        private static void Log(string text)
        {
            Outputter.Output(text);
        }
    }
}
