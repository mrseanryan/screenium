//Copyright (c) 2015 Sean Ryan
//
//See the file license.txt for copying permission.

using System;

namespace screenium.Csv
{
    public class TestConfigReaderException : Exception
    {
        public TestConfigReaderException(int row, Exception innerException)
            : base("CSV error at row " + row, innerException)
        {
        }
    }
}
