using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Parachute.Exceptions;
using Parachute.Utilities;

namespace Parachute.Tests
{
    [TestClass]
    public class ArgumentTests
    {
        [TestMethod]
        [ExpectedException(typeof(ParachuteAbortException))]
        public void Pass_Version_Argument_Should_Return_Null()
        {
            var args = new[] { "--version" };
            var ap = new ArgumentParser(args);
            var settings = ap.ParseSettings();
        }

        [TestMethod]
        [ExpectedException(typeof(ParachuteAbortException))]
        public void Pass_Help_Argument_Should_Return_Null()
        {
            var args = new[] { "--help" };
            var ap = new ArgumentParser(args);
            var settings = ap.ParseSettings();
        }

        [TestMethod]
        public void Pass_Setup_Argument_Should_Return_True()
        {
            var args = new[] { "--setup" };
            var ap = new ArgumentParser(args);
            var settings = ap.ParseSettings();
            Assert.IsNotNull(settings);
            Assert.AreEqual(true, settings.SetupDatabase);
        }

        [TestMethod]
        public void Pass_Source_Argument_Should_Return_Value()
        {
            var args = new[] { "--configfile", "C:\\Path\\To\\Files\\" };
            var ap = new ArgumentParser(args);
            var settings = ap.ParseSettings();
            Assert.IsNotNull(settings);
            Assert.AreEqual("C:\\Path\\To\\Files\\", settings.ConfigFilePath);
        }

        [TestMethod]
        public void Pass_Short_Source_Argument_Should_Return_Value()
        {
            var args = new[] { "-f", "C:\\Path\\To\\Files\\" };
            var ap = new ArgumentParser(args);
            var settings = ap.ParseSettings();
            Assert.IsNotNull(settings);
            Assert.AreEqual("C:\\Path\\To\\Files\\", settings.ConfigFilePath);
        }

        [TestMethod]
        public void Pass_Server_Argument_Should_Return_Value()
        {
            var args = new[] { "--server", "SERVERNAME" };
            var ap = new ArgumentParser(args);
            var settings = ap.ParseSettings();
            Assert.IsNotNull(settings);
            Assert.AreEqual("SERVERNAME", settings.Server);
        }

        [TestMethod]
        public void Pass_Short_Server_Argument_Should_Return_Value()
        {
            var args = new[] { "-s", "SERVERNAME" };
            var ap = new ArgumentParser(args);
            var settings = ap.ParseSettings();
            Assert.IsNotNull(settings);
            Assert.AreEqual("SERVERNAME", settings.Server);
        }

        [TestMethod]
        public void Pass_Database_Argument_Should_Return_Value()
        {
            var args = new[] { "--database", "DATABASE" };
            var ap = new ArgumentParser(args);
            var settings = ap.ParseSettings();
            Assert.IsNotNull(settings);
            Assert.AreEqual("DATABASE", settings.Database);
        }

        [TestMethod]
        public void Pass_Short_Database_Argument_Should_Return_Value()
        {
            var args = new[] { "-d", "DATABASE" };
            var ap = new ArgumentParser(args);
            var settings = ap.ParseSettings();
            Assert.IsNotNull(settings);
            Assert.AreEqual("DATABASE", settings.Database);
        }

        [TestMethod]
        public void Pass_Username_Argument_Should_Return_Value()
        {
            var args = new[] { "--username", "USERNAME" };
            var ap = new ArgumentParser(args);
            var settings = ap.ParseSettings();
            Assert.IsNotNull(settings);
            Assert.AreEqual("USERNAME", settings.Username);
        }

        [TestMethod]
        public void Pass_Short_Username_Argument_Should_Return_Value()
        {
            var args = new[] { "-u", "USERNAME" };
            var ap = new ArgumentParser(args);
            var settings = ap.ParseSettings();
            Assert.IsNotNull(settings);
            Assert.AreEqual("USERNAME", settings.Username);
        }

        [TestMethod]
        public void Pass_Password_Argument_Should_Return_Value()
        {
            var args = new[] { "--password", "PASSWORD" };
            var ap = new ArgumentParser(args);
            var settings = ap.ParseSettings();
            Assert.IsNotNull(settings);
            Assert.AreEqual("PASSWORD", settings.Password);
        }

        [TestMethod]
        public void Pass_Short_Password_Argument_Should_Return_Value()
        {
            var args = new[] { "-p", "PASSWORD" };
            var ap = new ArgumentParser(args);
            var settings = ap.ParseSettings();
            Assert.IsNotNull(settings);
            Assert.AreEqual("PASSWORD", settings.Password);
        }

        [TestMethod]
        public void Pass_ConnectionString_Argument_Should_Return_Value()
        {
            var args = new[] { "--connection", "CONNSTR" };
            var ap = new ArgumentParser(args);
            var settings = ap.ParseSettings();
            Assert.IsNotNull(settings);
            Assert.AreEqual("CONNSTR", settings.ConnectionString);
        }

        [TestMethod]
        public void Pass_Short_ConnectionString_Argument_Should_Return_Value()
        {
            var args = new[] { "-c", "CONNSTR" };
            var ap = new ArgumentParser(args);
            var settings = ap.ParseSettings();
            Assert.IsNotNull(settings);
            Assert.AreEqual("CONNSTR", settings.ConnectionString);
        }
    }
}
