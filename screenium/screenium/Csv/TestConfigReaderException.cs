//original license: MIT
//
//See the file license.txt for copying permission.

using System;

namespace screenium.Csv
{
    public class TestConfigReaderException : Exception
    {
        public TestConfigReaderException(int row, int column, Exception innerException)
            : base("CSV error at row " + row + " column " + column, innerException)
        {
        }
    }
}
