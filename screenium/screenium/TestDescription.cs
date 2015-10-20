//Copyright (c) 2015 Sean Ryan
//
//See the file license.txt for copying permission.

using System.Collections.Generic;
namespace screenium
{
    /// <summary>
    /// describes 1 test from a CSV file
    /// </summary>
    class TestDescription
    {
        //testName, testDescription, divSelector, Url, TitleContains
        internal string Name;
        internal string Description;
        internal string DivSelector;
        internal string Url;
        internal string TitleContains;

        internal static System.Collections.Generic.List<TestDescription> GetTestsByName(List<TestDescription> tests, string name)
        {
            if (name.ToLowerInvariant().CompareTo("all") == 0)
            {
                return tests;
            }

            var testsToRun = new List<TestDescription>();

            name = name.ToLowerInvariant();

            foreach (var test in tests)
            {
                if (test.Name.ToLowerInvariant().CompareTo(name) == 0)
                {
                    testsToRun.Add(test);
                }
            }

            return testsToRun;
        }
    }
}
