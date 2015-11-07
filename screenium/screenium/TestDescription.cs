//original license: MIT
//
//See the file license.txt for copying permission.

using System;
using System.Collections.Generic;
using System.Drawing;

namespace screenium
{
    /// <summary>
    /// describes 1 test from a CSV file
    /// </summary>
    public class TestDescription
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string DivSelector { get; set; }
        public string Url { get; set; }
        public string Query1 { get; set; }
        public string Query2 { get; set; }
        public string Query3 { get; set; }
        public string TitleContains { get; set; }
        public int CropAdjustWidth { get; set; }
        public int CropAdjustHeight { get; set; }
        public double Tolerance { get; set; }
        public TimeSpan SleepTimespan { get; set; }

        public static List<TestDescription> GetTestsByName(List<TestDescription> tests, string name)
        {
            if (string.Compare(name, "all", StringComparison.InvariantCultureIgnoreCase) == 0)
            {
                return tests;
            }

            var testsToRun = new List<TestDescription>();

            name = name.ToLowerInvariant();

            foreach (var test in tests)
            {
                if (string.Compare(test.Name, name, StringComparison.InvariantCultureIgnoreCase) == 0)
                {
                    testsToRun.Add(test);
                }
            }

            if (testsToRun.Count == 0)
            {
                throw new ArgumentException("No tests were found in the CSV file that match the test name '" + name + "'");
            }

            return testsToRun;
        }

        public int WindowWidth { get; set; }
        
        public int WindowHeight { get; set; }

        public Size WindowSize 
        {
            get { return new Size(WindowWidth, WindowHeight); }
        }
    }
}
