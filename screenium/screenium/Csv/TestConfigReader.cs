//Copyright (c) 2015 Sean Ryan
//
//See the file license.txt for copying permission.

using System;
using System.Collections.Generic;

namespace screenium.Csv
{
    class TestConfigReader
    {
        internal List<TestDescription> ReadFromFilePath(string path)
        {
            List<TestDescription> tests = new List<TestDescription>();

            using (var reader = new GenericCsvFileReader(path))
            {
                int column = 0;
                try
                {
                    CsvRow row = new CsvRow();
                    while (reader.ReadRow(row))
                    {
                        if (row.Count == 0)
                        {
                            continue;
                        }

                        column = 0;
                        TestDescription test = new TestDescription()
                        {
                            Name = CleanText(row[column++]),
                            Description = CleanText(row[column++]),
                            DivSelector = CleanText(row[column++]),
                            Url = CleanText(row[column++]),
                            Query1 = CleanText(row[column++]),
                            Query2 = CleanText(row[column++]),
                            Query3 = CleanText(row[column++]),
                            TitleContains = CleanText(row[column++]),
                            WindowWidth = CleanTextAsInt(row[column++]),
                            WindowHeight = CleanTextAsInt(row[column++]),
                            CropAdjustWidth = CleanTextAsInt(row[column++]),
                            CropAdjustHeight = CleanTextAsInt(row[column++]),
                            Tolerance = CleanTextAsDouble(row[column++])
                        };
                        tests.Add(test);
                    }
                }
                catch (Exception ex)
                {
                    throw new TestConfigReaderException(reader.CurrentRow, column + 1, ex);
                }
            }

            return tests;
        }

        private double CleanTextAsDouble(string text)
        {
            return double.Parse(CleanText(text));
        }

        private int CleanTextAsInt(string text)
        {
            return int.Parse(CleanText(text));
        }

        private string CleanText(string text)
        {
            return text.Trim();
        }
    }
}
