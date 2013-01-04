using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Parachute.Entities;

namespace Parachute.Tests
{
    [TestClass]
    public class SchemaVersionTest
    {
        [TestMethod]
        public void Constructor_PassingValidData_ShouldInstantiateObject()
        {
            var sv = new SchemaVersion("01", "01", "0123");
            Assert.AreEqual(typeof(SchemaVersion), sv.GetType());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_PassingInvalidMajorRevision_ShouldInstantiateObject()
        {
            var sv = new SchemaVersion("0XX", "01", "0123");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_PassingInvalidMinorRevision_ShouldInstantiateObject()
        {
            var sv = new SchemaVersion("01", "0XX", "0123");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_PassingInvalidPointReleaseNumber_ShouldInstantiateObject()
        {
            var sv = new SchemaVersion("01", "01", "01X3");
        }

        [TestMethod]
        public void OperatorOverload_LessThan_WhenFirstLessThanSecond_ShouldReturnTrue()
        {
            var first = new SchemaVersion("01", "01", "0123");
            var second = new SchemaVersion("01", "01", "0124");

            Assert.IsTrue(first < second);
        }

        [TestMethod]
        public void OperatorOverload_LessThanOrEqual_WhenFirstLessThanSecond_ShouldReturnTrue()
        {
            var first = new SchemaVersion("01", "01", "0123");
            var second = new SchemaVersion("01", "01", "0124");

            Assert.IsTrue(first <= second);
        }

        [TestMethod]
        public void OperatorOverload_LessThanOrEqual_WhenFirstEqualToSecond_ShouldReturnTrue()
        {

            var first = new SchemaVersion("01", "01", "0123");
            var second = new SchemaVersion("01", "01", "0123");

            Assert.IsTrue(first <= second);
        }

        [TestMethod]
        public void OperatorOverload_GreaterThan_WhenFirstGreaterThanSecond_ShouldReturnTrue()
        {
            var first = new SchemaVersion("01", "01", "0125");
            var second = new SchemaVersion("01", "01", "0124");

            Assert.IsTrue(first > second);
        }

        [TestMethod]
        public void OperatorOverload_GreaterThanOrEqual_WhenFirstGreaterThanSecond_ShouldReturnTrue()
        {
            var first = new SchemaVersion("01", "01", "0125");
            var second = new SchemaVersion("01", "01", "0124");

            Assert.IsTrue(first >= second);
        }

        [TestMethod]
        public void OperatorOverload_GreaterThanOrEqual_WhenFirstEqualToSecond_ShouldReturnTrue()
        {
            var first = new SchemaVersion("01", "01", "0124");
            var second = new SchemaVersion("01", "01", "0124");

            Assert.IsTrue(first >= second);
        }

        [TestMethod]
        public void OperatorOverload_Equals_WhenFirstEqualToSecond_ShouldReturnTrue()
        {
            var first = new SchemaVersion("01", "01", "0125");
            var second = new SchemaVersion("01", "01", "0125");

            Assert.IsTrue(first == second);
        }

        [TestMethod]
        public void OperatorOverload_Equals_WhenFirstNotEqualToSecond_ShouldReturnFalse()
        {
            var first = new SchemaVersion("01", "01", "0125");
            var second = new SchemaVersion("01", "01", "0124");

            Assert.IsFalse(first == second);
        }

        [TestMethod]
        public void OperatorOverload_NotEquals_WhenFirsNotEqualToSecond_ShouldReturnTrue()
        {
            var first = new SchemaVersion("01", "01", "0125");
            var second = new SchemaVersion("01", "01", "0124");

            Assert.IsTrue(first != second);
        }

        [TestMethod]
        public void OperatorOverload_NotEquals_WhenFirstNotEqualToSecond_ShouldReturnFalse()
        {
            var first = new SchemaVersion("01", "01", "0125");
            var second = new SchemaVersion("01", "01", "0125");

            Assert.IsFalse(first != second);
        }
    }
}
