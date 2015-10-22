﻿//Copyright (c) 2015 Sean Ryan
//
//See the file license.txt for copying permission.

using System.Collections.Generic;

namespace screenium
{
    class CsvReader
    {
        internal List<TestDescription> ReadFromFilePath(string path)
        {
            List<TestDescription> tests = new List<TestDescription>();

            using (var reader = new CsvFileReader(path))
            {
                CsvRow row = new CsvRow();
                while (reader.ReadRow(row))
                {
                    if (row.Count == 0)
                    {
                        continue;
                    }

                    int column = 0;
                    TestDescription test = new TestDescription()
                    {
                        Name = CleanText(row[column++]),
                        Description = CleanText(row[column++]),
                        DivSelector = CleanText(row[column++]),
                        Url = CleanText(row[column++]),
                        Query1 = CleanText(row[column++]),
                        Query2 = CleanText(row[column++]),
                        Query3 = CleanText(row[column++]),
                        TitleContains = CleanText(row[column++])
                    };
                    tests.Add(test);
                }
            }

            return tests;
        }

        private string CleanText(string text)
        {
            return text.Trim();
        }
    }
}
