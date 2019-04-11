using System;
using Xunit;

namespace XUnitTestProject1
{
    public class UnitTest1
    {
        [Fact]
        public void TestFileFormatParsing()
        {
            bool isCSV;

            ForkCSVfile.Program.GetFormat("CSV", out isCSV);
            Assert.Equal(true, isCSV);
        }
    }
}
