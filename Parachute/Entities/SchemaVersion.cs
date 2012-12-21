using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parachute.Entities
{
    public class SchemaVersion : IComparable<SchemaVersion>
    {
        private static RegexStringValidator _majorMinorValidator = new RegexStringValidator(@"\d{2}");
        private static RegexStringValidator _pointReleaseValidator = new RegexStringValidator(@"\d{4}");

        public SchemaVersion(string majorVersion, string minorVersion, string pointRelease)
        {
            _majorMinorValidator.Validate(majorVersion);
            _majorMinorValidator.Validate(minorVersion);
            _pointReleaseValidator.Validate(pointRelease);
            
            MajorVersion = majorVersion;
            MinorVersion = minorVersion;
            PointRelease = pointRelease;
        }
        
        public string MajorVersion { get; private set; }

        public string MinorVersion { get; private set; }

        public string PointRelease { get; private set; }

        private int InternalNumericRepresentation
        {
            get
            {
                var mj = int.Parse(MajorVersion);
                var mn = int.Parse(MinorVersion);
                var pr = int.Parse(PointRelease);
                
                return (mj*1000000) + (mn*10000) + pr;
            }
        }

        public string Version
        {
            get { return string.Format("{0}.{1}.{2}", MajorVersion, MinorVersion, PointRelease); }
        }

        public static SchemaVersion MinValue
        {
            get { return new SchemaVersion("00", "00", "0000"); }
        }

        public static SchemaVersion MaxValue
        {
            get { return new SchemaVersion("99", "99", "9999"); }
        }

        public int CompareTo(SchemaVersion other)
        {
            if (InternalNumericRepresentation < other.InternalNumericRepresentation)
                return -1;

            if (InternalNumericRepresentation > other.InternalNumericRepresentation)
                return 1;
            
            return 0;
        }

        public override int GetHashCode()
        {
            return InternalNumericRepresentation;
        }

        public override bool Equals(object obj)
        {
            if(obj == null || (obj as SchemaVersion) == null) return false;

            return InternalNumericRepresentation == (obj as SchemaVersion).InternalNumericRepresentation;
        }

        public override string ToString()
        {
            return Version;
        }

        public static bool operator <(SchemaVersion first, SchemaVersion second)
        {
            return first.CompareTo(second) < 0;
        }

        public static bool operator >(SchemaVersion first, SchemaVersion second)
        {
            return first.CompareTo(second) > 0;
        }

        public static bool operator <=(SchemaVersion first, SchemaVersion second)
        {
            return first.CompareTo(second) < 0 || first.Equals(second);
        }

        public static bool operator >=(SchemaVersion first, SchemaVersion second)
        {
            return first.CompareTo(second) > 0 || first.Equals(second);
        }

        public  static bool operator == (SchemaVersion first, SchemaVersion second)
        {
            return first.Equals(second);
        }

        public static bool operator !=(SchemaVersion first, SchemaVersion second)
        {
            return !first.Equals(second);
        }
    }
}
