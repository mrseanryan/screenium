//Copyright (c) 2015 Sean Ryan
//
//See the file license.txt for copying permission.

//altered code, is now under MIT.
//
//original license:
// licensed under The Code Project Open License (CPOL)
//ref: http://www.codeproject.com/Articles/415732/Reading-and-Writing-CSV-Files-in-Csharp

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

                    TestDescription test = new TestDescription()
                    {
                        Name = CleanText(row[0]),
                        Description = CleanText(row[1]),
                        DivSelector = CleanText(row[2]),
                        Url = CleanText(row[3]),
                        TitleContains = CleanText(row[4])
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
