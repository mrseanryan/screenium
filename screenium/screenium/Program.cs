//Copyright (c) 2015 Sean Ryan
//
//See the file license.txt for copying permission.

using System;
using System.Collections.Generic;
using screenium.Csv;
using screenium.SeleniumIntegration;
using screenium.Compare;

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
                    //TODO return code of 666 if one or more tests detected differences
                    var result = RunTests(argProc);
                    return GetResultAsReturnCode(result);
                }
            }
            catch (Exception ex)
            {
                Outputter.Output(ex);
                return 2;
            }
            return 0;
        }

        private static int GetResultAsReturnCode(CompareResult result)
        {
            switch (result)
            {
                case CompareResult.Similar:
            return 0;
                case CompareResult.Different:
                    return 666;
                default:
                    throw new ArgumentException("not a recognised CompareResult: " + result);
            }
        }

        private static CompareResult RunTests(ArgsProcessor argProc)
        {
            List<TestDescription> testsToRun = ReadTests(argProc);

            var runner = new TestRunner();
            var result = runner.RunTests(testsToRun, argProc);

            Log("Finished running tests [" + GetResultAsString(result) + "]");
            return result;
        }

        private static string GetResultAsString(CompareResult result)
        {
            switch (result)
            {
                case CompareResult.Similar:
                    return "OK";
                case CompareResult.Different:
                    return "Differences found";
                default:
                    throw new ArgumentException("not a recognised CompareResult: " + result);
            }
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
