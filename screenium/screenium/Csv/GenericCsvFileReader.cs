//original license: MIT
//
//See the file license.txt for copying permission.

//altered code, is now under MIT.
//
//original license:
// licensed under The Code Project Open License (CPOL)
//ref: http://www.codeproject.com/Articles/415732/Reading-and-Writing-CSV-Files-in-Csharp

using System;
using System.Collections.Generic;
using System.IO;

namespace screenium.Csv
{
    /// <summary>
    /// Class to store one CSV row
    /// </summary>
    public class CsvRow : List<string>
    {
        public string LineText { get; set; }
    }

    /// <summary>
    /// Class to read data from a CSV file
    /// </summary>
    internal class GenericCsvFileReader : StreamReader
    {
        private int _currentRow = 0;

        //TODO CSV reader - use StreamReader by encapsulation not inheritance

        public GenericCsvFileReader(Stream stream)
            : base(stream)
        {
        }

        public GenericCsvFileReader(string filename)
            : base(filename)
        {
        }

        /// <summary>
        /// Reads a row of data from a CSV file
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public bool ReadRow(CsvRow row)
        {
            while (true)
            {
                row.LineText = ReadLine();
                _currentRow++;
                if (String.IsNullOrEmpty(row.LineText))
                {
                    return false;
                }
                if (!row.LineText.StartsWith("#"))
                {
                    break;
                }
            }

            int pos = 0;
            int rows = 0;

            while (pos < row.LineText.Length)
            {
                string value;

                // Special handling for quoted field
                if (row.LineText[pos] == '"')
                {
                    // Skip initial quote
                    pos++;

                    // Parse quoted value
                    int start = pos;
                    while (pos < row.LineText.Length)
                    {
                        // Test for quote character
                        if (row.LineText[pos] == '"')
                        {
                            // Found one
                            pos++;

                            // If two quotes together, keep one
                            // Otherwise, indicates end of value
                            if (pos >= row.LineText.Length || row.LineText[pos] != '"')
                            {
                                pos--;
                                break;
                            }
                        }
                        pos++;
                    }
                    value = row.LineText.Substring(start, pos - start);
                    value = value.Replace("\"\"", "\"");
                }
                else
                {
                    // Parse unquoted value
                    int start = pos;
                    while (pos < row.LineText.Length && row.LineText[pos] != ',')
                        pos++;
                    value = row.LineText.Substring(start, pos - start);
                }

                // Add field to list
                if (rows < row.Count)
                    row[rows] = value;
                else
                    row.Add(value);
                rows++;

                // Eat up to and including next comma
                while (pos < row.LineText.Length && row.LineText[pos] != ',')
                    pos++;
                if (pos < row.LineText.Length)
                    pos++;
            }
            // Delete any unused items
            while (row.Count > rows)
                row.RemoveAt(rows);

            // Return true if any columns read
            return (row.Count > 0);
        }

        public int CurrentRow
        {
            get
            {
                return this._currentRow;
            }
        }
    }
}
