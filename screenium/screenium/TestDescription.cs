//Copyright (c) 2015 Sean Ryan
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
    class TestDescription
    {
        internal string Name { get; set; }
        internal string Description { get; set; }
        internal string DivSelector { get; set; }
        internal string Url { get; set; }
        internal string Query1 { get; set; }
        internal string Query2 { get; set; }
        internal string Query3 { get; set; }
        internal string TitleContains { get; set; }
        internal int CropAdjustWidth { get; set; }
        internal int CropAdjustHeight { get; set; }

        internal static List<TestDescription> GetTestsByName(List<TestDescription> tests, string name)
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
