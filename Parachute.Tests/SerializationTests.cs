using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Parachute.Entities;

namespace Parachute.Tests
{
    [TestClass]
    public class SerializationTests
    {

        public const string strVariableXml = @"<?xml version=""1.0"" encoding=""utf-16""?>
<variable key=""var1"" defaultValue=""somestring"" />";

        public const string strScriptXml = @"<?xml version=""1.0"" encoding=""utf-16""?>
<script scriptName=""test"">
  <variables>
    <variable key=""var1"" defaultValue=""somestring"" />
    <variable key=""var1"" defaultValue=""somestring"" />
    <variable key=""var1"" defaultValue=""somestring"" />
  </variables>
</script>";

        public const string strScriptLocationsXml = @"<?xml version=""1.0"" encoding=""utf-16""?>
<scriptLocation recursive=""false"" containsSchemaScripts=""false"" runOnce=""true"" path=""..\Some\Path"">
  <script scriptName=""test"">
    <variables>
      <variable key=""var1"" defaultValue=""somestring"" />
    </variables>
  </script>
  <script scriptName=""test"">
    <variables>
      <variable key=""var1"" defaultValue=""somestring"" />
    </variables>
  </script>
  <script scriptName=""test"">
    <variables>
      <variable key=""var1"" defaultValue=""somestring"" />
    </variables>
  </script>
</scriptLocation>"; 

        public const string strScriptInformationXml = @"<?xml version=""1.0"" encoding=""utf-16""?>
<scriptInfo>
  <scriptLocations recursive=""false"" containsSchemaScripts=""true"" runOnce=""true"" path=""..\Some\Path"">
    <script scriptName=""test"">
      <variables>
        <variable key=""var1"" defaultValue=""somestring"" />
      </variables>
    </script>
  </scriptLocations>
  <scriptLocations recursive=""false"" containsSchemaScripts=""true"" runOnce=""true"" path=""..\Some\Path"">
    <script scriptName=""test"">
      <variables>
        <variable key=""var1"" defaultValue=""somestring"" />
      </variables>
    </script>
  </scriptLocations>
  <scriptLocations recursive=""false"" containsSchemaScripts=""true"" runOnce=""true"" path=""..\Some\Path"">
    <script scriptName=""test"">
      <variables>
        <variable key=""var1"" defaultValue=""somestring"" />
      </variables>
    </script>
  </scriptLocations>
</scriptInfo>";



        [TestMethod]
        public void Can_Serialize_Variable()
        {
            var xns = new XmlSerializerNamespaces();
            xns.Add("","");
            var v = new Variable { Key = "var1", DefaultValue = "somestring" };
            var xs = new XmlSerializer(typeof(Variable));
            var sb = new StringBuilder();
            var sw = new StringWriter(sb);
            xs.Serialize(sw, v, xns);

            Debug.WriteLine(sb.ToString());
            Assert.AreEqual(strVariableXml, sb.ToString());
        }


        [TestMethod]
        public void Can_Deserialize_Variable()
        {
            var xs = new XmlSerializer(typeof(Variable));
            var sr = new StringReader(strVariableXml);
            var v = xs.Deserialize(sr) as Variable;

            Assert.IsNotNull(v);
            Assert.AreEqual(v.Key, "var1");
            Assert.AreEqual(v.DefaultValue, "somestring");
        }


        [TestMethod]
        public void Can_Serialize_Script()
        {
            var xns = new XmlSerializerNamespaces();
            xns.Add("", "");
            var v = new Variable { Key = "var1", DefaultValue = "somestring" };
            var s = new Script {ScriptName = "test"};
            s.Variables.Add(v);
            s.Variables.Add(v);
            s.Variables.Add(v);
                
            var xs = new XmlSerializer(typeof(Script));
            var sb = new StringBuilder();
            var sw = new StringWriter(sb);
            xs.Serialize(sw, s, xns);

            Debug.WriteLine(sb.ToString());
            Assert.AreEqual(strScriptXml, sb.ToString());
        }

        [TestMethod]
        public void Can_Deserialize_Script()
        {
            var xs = new XmlSerializer(typeof(Script));
            var sr = new StringReader(strScriptXml);
            var s = xs.Deserialize(sr) as Script;

            Assert.IsNotNull(s);
            Assert.IsNotNull(s.Variables);
            Assert.AreEqual(3, s.Variables.Count);
        }

        [TestMethod]
        public void Can_Serialize_ScriptLocations()
        {
            var xns = new XmlSerializerNamespaces();
            xns.Add("", "");
            var v = new Variable { Key = "var1", DefaultValue = "somestring" };
            var s = new Script { ScriptName = "test" };
            s.Variables.Add(v);

            var sl = new ScriptLocation
                {
                    Path = "..\\Some\\Path",
                    Recursive = false,
                    RunOnce = true,
                };
            sl.Scripts.Add(s);
            sl.Scripts.Add(s);
            sl.Scripts.Add(s);

            var xs = new XmlSerializer(typeof(ScriptLocation));
            var sb = new StringBuilder();
            var sw = new StringWriter(sb);
            xs.Serialize(sw, sl,xns);

            Debug.WriteLine(sb.ToString());
            Assert.AreEqual(strScriptLocationsXml, sb.ToString());
        }

        [TestMethod]
        public void Can_Deserialize_ScriptLocations()
        {
            var xs = new XmlSerializer(typeof(ScriptLocation));
            var sr = new StringReader(strScriptLocationsXml);
            var s = xs.Deserialize(sr) as ScriptLocation;

            Assert.IsNotNull(s);
            Assert.IsNotNull(s.Scripts);
            Assert.AreEqual(3, s.Scripts.Count);
        }

        [TestMethod]
        public void Can_Serialize_ScriptInformation()
        {
            var xns = new XmlSerializerNamespaces();
            xns.Add("", "");
            var v = new Variable { Key = "var1", DefaultValue = "somestring" };
            var s = new Script { ScriptName = "test" };
            s.Variables.Add(v);

            var sl = new ScriptLocation
            {
                Path = "..\\Some\\Path",
                Recursive = false,
                RunOnce = true,
                ContainsSchemaScripts = true
            };
            sl.Scripts.Add(s);

            var si = new ScriptInformation();
            si.ScriptLocations.Add(sl);
            si.ScriptLocations.Add(sl);
            si.ScriptLocations.Add(sl);

            var xs = new XmlSerializer(typeof(ScriptInformation));
            var sb = new StringBuilder();
            var sw = new StringWriter(sb);
            xs.Serialize(sw, si, xns);

            Debug.WriteLine(sb.ToString());
            Assert.AreEqual(strScriptInformationXml, sb.ToString());
        }

        [TestMethod]
        public void Can_Deserialize_ScriptInfomration()
        {
            var xs = new XmlSerializer(typeof(ScriptInformation));
            var sr = new StringReader(strScriptInformationXml);
            var s = xs.Deserialize(sr) as ScriptInformation;

            Assert.IsNotNull(s);
            Assert.IsNotNull(s.ScriptLocations);
            Assert.AreEqual(3, s.ScriptLocations.Count);

        }

    }
}
