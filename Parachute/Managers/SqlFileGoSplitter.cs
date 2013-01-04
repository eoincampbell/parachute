using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Parachute.Managers
{
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