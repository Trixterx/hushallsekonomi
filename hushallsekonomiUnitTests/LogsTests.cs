using NUnit.Framework;
using System.IO;
using hushallsekonomiClasses.Helpers;
using NUnit.Framework.Internal;

namespace hushallsekonomiUnitTests
{
    public class LogsTests
    {
        [SetUp]
        public void Setup()
        {
            Logging.Log("Hej");
        }

        [Test]
        public void TestLogsFileExist()
        {
            Assert.AreEqual(true, File.Exists(Logging.Filename));
        }
    }
}