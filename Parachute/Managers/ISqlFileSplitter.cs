using System.Collections.Generic;

namespace Parachute.Managers
{
    public interface ISqlFileSplitter
    {
        IEnumerable<string> Split(string sql);
    }
}