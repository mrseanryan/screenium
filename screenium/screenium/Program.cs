﻿//original license: MIT
//
//See the file license.txt for copying permission.

using screenium.Compare;
using screenium.Csv;
using screenium.SeleniumIntegration;
using System;
using System.Collections.Generic;

namespace screenium
{
    /// <summary>
    /// the top level command line program class.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// the main entry point for the process.
        /// </summary>
        public static int Main(string[] args)
        {
            var isVerbose = false;
            try
            {
                ArgsProcessor argProc = new ArgsProcessor(args);

                string message;
                if (!argProc.Validate(out message))
                {
                    Console.WriteLine(message);
                    Console.WriteLine(argProc.GetUsage());
                    return 1;
                }

                isVerbose = argProc.IsOptionOn(ArgsProcessor.Options.Verbose);

                if (argProc.IsOptionOn(ArgsProcessor.Options.TestSelf))
                {
                    RunSelfTest();
                }
                else if (argProc.IsOptionOn(ArgsProcessor.Options.AcceptChanges))
                {
                    ConvertActualToExpected(argProc);
                }
                else

                {
                    var result = RunTests(argProc);
                    return GetResultAsReturnCode(result);
                }
            }
            catch (Exception ex)
            {
                Outputter.Output(ex, isVerbose);
                return 2;
            }
            return 0;
        }

        private static void ConvertActualToExpected(ArgsProcessor argProc)
        {
            //protect the saved 'expected' files, by prompting the user:
            var message = "Accept recent changes as new 'expected' images? [Y to continue]";
            if (!IsUserOkToContinue(message))
            {
                Outputter.Output("Cancelling...");
                return;
            }

            var testsToRun = ReadTests(argProc);
            var converter = new ActualToExpectedConverter(argProc);
            converter.Convert(testsToRun);
        }

        private static int GetResultAsReturnCode(CompareResult result)
        {
            switch (result)
            {
                case CompareResult.Similar:
                    return 0;
                case CompareResult.Different:
                    return 666;
                case CompareResult.Cancelled:
                    return 404;
                default:
                    throw new ArgumentException("not a recognised CompareResult: " + result);
            }
        }

        private static CompareResult RunTests(ArgsProcessor argProc)
        {
            List<TestDescription> testsToRun = ReadTests(argProc);

            if (argProc.IsOptionOn(ArgsProcessor.Options.Save))
            {
                //protect the saved 'expected' files, by prompting the user:
                var message = "Get new saved 'expected' images? [Y to continue]";
                if (!IsUserOkToContinue(message))
                {
                    Outputter.Output("Cancelling...");
                    return CompareResult.Cancelled;
                }
            }

            Outputter.OutputSeparator();
            var runner = new TestRunner();
            var result = runner.RunTests(testsToRun, argProc);

            Outputter.Output("Finished running tests:");
            Outputter.Output("[" + CompareResultHelper.GetResultAsString(result) + "]", CompareResultHelper.GetResultAsConsoleColor(result));
            return result;
        }

        private static bool IsUserOkToContinue(string message)
        {
            Outputter.Output(message);
            var keyInfo = Console.ReadKey();
            Console.WriteLine();
            bool isOk = keyInfo.KeyChar.ToString().ToLowerInvariant() == "y";
            return isOk;
        }

        private static void RunSelfTest()
        {
            Outputter.Output("Running self test ...");
            using (BrowserDriver driver = new BrowserDriver())
            {
                driver.TestChrome();
            }
            Outputter.Output("self test ran [OK]");
        }

        private static List<TestDescription> ReadTests(ArgsProcessor argProc)
        {
            List<TestDescription> testsToRun;
            List<TestDescription> tests;
            {
                var reader = new TestConfigReader();
                var path = argProc.GetArg(ArgsProcessor.Args.CSV_FILE_PATH);
                tests = reader.ReadFromFilePath(path);
                Outputter.Output(string.Format("Read {0} tests from Test Suite: {1}", tests.Count, path));
            }

            testsToRun = TestDescription.GetTestsByName(tests, argProc.GetArg(ArgsProcessor.Args.TEST_NAME));
            return testsToRun;
        }
    }
}
