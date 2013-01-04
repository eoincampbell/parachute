using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Parachute.Entities;

namespace Parachute.Tests
{

    [TestClass]
    public class ScriptLocationTests
    {
        private const string Expected = "D:\\Temp\\";

        [ClassInitialize]
        public static void Initialize(TestContext context)
        {
            if(!Directory.Exists(Expected))
            {
                Directory.CreateDirectory(Expected);
            }
        }

        [TestMethod]
        public void CreateScriptLocation_WhenPathIsAbsolute_PathFilterShouldBeSame()
        {
            var sl = new ScriptLocation
                {
                    Path = Expected
                };


            Assert.AreEqual(Expected, sl.AbsolutePath);
        }

        [TestMethod]
        public void CreateScriptLocation_WhenPathIsRelative_PathFilterShouldBeAbsolute()
        {
            var sl = new ScriptLocation
                {
                    Path = "..\\..\\..\\..\\..\\..\\..\\..\\..\\..\\Temp\\"
                };


            Assert.AreEqual(Expected, sl.AbsolutePath);
        }
    }
}
