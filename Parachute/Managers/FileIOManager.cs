using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Parachute.Managers
{
// ReSharper disable InconsistentNaming
    public class FileIOManager
// ReSharper restore InconsistentNaming
    {
        private ISqlFileSplitter Splitter { get; set; }

        public FileIOManager() : this(null)
        {
        }

        public FileIOManager(ISqlFileSplitter splitter)
        {
            Splitter = splitter;
        }

        public string GetSqlScriptFromFile(string path)
        {
            using (var strm = File.OpenRead(path))
            {
                using (var reader = new StreamReader(strm))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        public IEnumerable<string> ReadSqlScripts(string path)
        {
            var fileContents = GetSqlScriptFromFile(path);

            return (Splitter != null)
                       ? Splitter.Split(fileContents)
                       : new List<string> {fileContents};
        }
    }


    public interface ISqlFileSplitter
    {
        IEnumerable<string> Split(string sql);
    }

    public class SqlFileGoSplitter : ISqlFileSplitter
    {
        private static readonly Regex SplitterRegex = 
            new Regex("^\\s*GO\\s*$", RegexOptions.IgnoreCase | RegexOptions.Multiline);


        public IEnumerable<string> Split(string sql)
        {
            return SplitterRegex.Split(sql);
        }
    }
}
