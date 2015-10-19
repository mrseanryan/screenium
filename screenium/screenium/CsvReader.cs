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
                    TestDescription test = new TestDescription()
                    {
                        Name = row[0],
                        Description = row[1],
                        DivSelector = row[2],
                        Url = row[3]
                    };
                    tests.Add(test);
                }
            }

            return tests;
        }
    }
}
