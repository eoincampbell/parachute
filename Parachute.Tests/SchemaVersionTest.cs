using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Parachute.Entities;

namespace Parachute.Tests
{
    [TestClass]
    public class SchemaVersionTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var sv = new SchemaVersion("01", "01", "0123");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestMethod2()
        {
            var sv = new SchemaVersion("0XX", "01", "0123");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestMethod3()
        {
            var sv = new SchemaVersion("01", "0XX", "0123");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestMethod4()
        {
            var sv = new SchemaVersion("01", "01", "01X3");
        }

        [TestMethod]
        public void TestMethod5()
        {
            var first = new SchemaVersion("01", "01", "0123");
            var second = new SchemaVersion("01", "01", "0124");

            Assert.IsTrue(first < second);
        }

        [TestMethod]
        public void TestMethod6()
        {
            var first = new SchemaVersion("01", "01", "0123");
            var second = new SchemaVersion("01", "01", "0124");

            Assert.IsTrue(first <= second);
        }

        [TestMethod]
        public void TestMethod7()
        {

            var first = new SchemaVersion("01", "01", "0123");
            var second = new SchemaVersion("01", "01", "0123");

            Assert.IsTrue(first <= second);
        }

        [TestMethod]
        public void TestMethod8()
        {
            var first = new SchemaVersion("01", "01", "0125");
            var second = new SchemaVersion("01", "01", "0124");

            Assert.IsTrue(first > second);
        }

        [TestMethod]
        public void TestMethod9()
        {
            var first = new SchemaVersion("01", "01", "0125");
            var second = new SchemaVersion("01", "01", "0124");

            Assert.IsTrue(first >= second);
        }

        [TestMethod]
        public void TestMethod10()
        {
            var first = new SchemaVersion("01", "01", "0124");
            var second = new SchemaVersion("01", "01", "0124");

            Assert.IsTrue(first >= second);
        }

        [TestMethod]
        public void TestMethod11()
        {
            var first = new SchemaVersion("01", "01", "0125");
            var second = new SchemaVersion("01", "01", "0125");

            Assert.IsTrue(first == second);
        }

        [TestMethod]
        public void TestMethod12()
        {
            var first = new SchemaVersion("01", "01", "0125");
            var second = new SchemaVersion("01", "01", "0124");

            Assert.IsTrue(first != second);
        }
    }
}
