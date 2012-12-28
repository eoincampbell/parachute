using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Parachute.Exceptions;

namespace Parachute.Tests
{
    [TestClass]
    public class ExceptionTests
    {
        [TestMethod]
        public void CreateParachuteAbortException_SupplyNoParameters_MessageShouldBeDefault()
        {
            const string message = "Error in the application.";
            var p = new ParachuteAbortException();

            Assert.AreEqual(message, p.Message);
            Assert.IsTrue(p.GetType().BaseType == typeof(ParachuteException));
        }

        [TestMethod]
        public void CreateParachuteAbortException_SupplyMessageParameter_MessageShouldBeEqual()
        {
            const string message = "Hello World";

            var p = new ParachuteAbortException(message);

            Assert.AreEqual(message, p.Message);
            Assert.IsTrue(p.GetType().BaseType == typeof(ParachuteException));
        }

        [TestMethod]
        public void CreateParachuteAbortException_SupplyMessageAndInnerExceptionParameter_MessageAndInnerExceptionShouldBeEqual()
        {
            var innerEx = new ArgumentException("Inner Hello World");
            const string message = "Hello World";

            var p = new ParachuteAbortException(message, innerEx);

            Assert.AreEqual(message, p.Message);
            Assert.AreEqual(innerEx, p.InnerException);
            Assert.IsTrue(p.GetType().BaseType == typeof(ParachuteException));
        }

        [TestMethod]
        public void CreateParachuteException_SupplyNoParameters_MessageShouldBeDefault()
        {
            const string message = "Error in the application.";
            var p = new ParachuteException();

            Assert.AreEqual(message, p.Message);
            Assert.IsTrue(p.GetType().BaseType == typeof(ApplicationException));
        }

        [TestMethod]
        public void CreateParachuteException_SupplyMessageParameter_MessageShouldBeEqual()
        {
            const string message = "Hello World";

            var p = new ParachuteException(message);

            Assert.AreEqual(message, p.Message);
            Assert.IsTrue(p.GetType().BaseType == typeof(ApplicationException));
        }

        [TestMethod]
        public void CreateParachuteException_SupplyMessageAndInnerExceptionParameter_MessageAndInnerExceptionShouldBeEqual()
        {
            var innerEx = new ArgumentException("Inner Hello World");
            const string message = "Hello World";

            var p = new ParachuteException(message, innerEx);

            Assert.AreEqual(message, p.Message);
            Assert.AreEqual(innerEx, p.InnerException);
            Assert.IsTrue(p.GetType().BaseType == typeof(ApplicationException));
        }
    }
}
