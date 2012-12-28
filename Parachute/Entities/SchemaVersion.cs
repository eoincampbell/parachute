using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parachute.Entities
{
    /// <summary>
    /// SchemaVersion encapsulates the version information for the database.
    /// </summary>
    public class SchemaVersion : IComparable<SchemaVersion>
    {
        /// <summary>
        /// Regular Expression Validator for validating the Major Verison & Minor Version components of a Version String in XX.YY.ZZZZ Format
        /// </summary>
        private static readonly RegexStringValidator MajorMinorValidator = new RegexStringValidator(@"\d{2}");
        /// <summary>
        /// Regular Expression Validator for validating the Point Release Version component of a Version String in XX.YY.ZZZZ Format
        /// </summary>
        private static readonly RegexStringValidator PointReleaseValidator = new RegexStringValidator(@"\d{4}");

        /// <summary>
        /// Initializes a new instance of the <see cref="SchemaVersion" /> class.
        /// </summary>
        /// <param name="majorVersion">The major version.</param>
        /// <param name="minorVersion">The minor version.</param>
        /// <param name="pointRelease">The point release number.</param>
        public SchemaVersion(string majorVersion, string minorVersion, string pointRelease)
        {
            MajorMinorValidator.Validate(majorVersion);
            MajorMinorValidator.Validate(minorVersion);
            PointReleaseValidator.Validate(pointRelease);
            
            MajorVersion = majorVersion;
            MinorVersion = minorVersion;
            PointRelease = pointRelease;
        }

        /// <summary>
        /// Gets the major version.
        /// </summary>
        /// <value>
        /// The major version.
        /// </value>
        public string MajorVersion { get; private set; }

        /// <summary>
        /// Gets the minor version.
        /// </summary>
        /// <value>
        /// The minor version.
        /// </value>
        public string MinorVersion { get; private set; }

        /// <summary>
        /// Gets the point release.
        /// </summary>
        /// <value>
        /// The point release.
        /// </value>
        public string PointRelease { get; private set; }

        /// <summary>
        /// Gets the internal numeric representation.
        /// </summary>
        /// <value>
        /// The internal numeric representation of a version number as an integer created by combining all version components. Used for comparison.
        /// </value>
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

        /// <summary>
        /// Gets the version.
        /// </summary>
        /// <value>
        /// The human readable string representing the version.
        /// </value>
        public string Version
        {
            get { return string.Format("{0}.{1}.{2}", MajorVersion, MinorVersion, PointRelease); }
        }

        /// <summary>
        /// Gets the min value.
        /// </summary>
        /// <value>
        /// The min value.
        /// </value>
        public static SchemaVersion MinValue
        {
            get { return new SchemaVersion("00", "00", "0000"); }
        }

        /// <summary>
        /// Gets the max value.
        /// </summary>
        /// <value>
        /// The max value.
        /// </value>
        public static SchemaVersion MaxValue
        {
            get { return new SchemaVersion("99", "99", "9999"); }
        }

        /// <summary>
        /// Compares one SchemaVersion to anohter
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns>An integer specifying whether this schema version is less than, greater than or equal to another</returns>
        public int CompareTo(SchemaVersion other)
        {
            if (InternalNumericRepresentation < other.InternalNumericRepresentation)
                return -1;

            if (InternalNumericRepresentation > other.InternalNumericRepresentation)
                return 1;
            
            return 0;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return InternalNumericRepresentation;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if(obj == null || (obj as SchemaVersion) == null) return false;

            return InternalNumericRepresentation == (obj as SchemaVersion).InternalNumericRepresentation;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
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
            return first != null && first.Equals(second);
        }

        public static bool operator !=(SchemaVersion first, SchemaVersion second)
        {
            return first != null && !first.Equals(second);
        }

        /// <summary>
        /// Parses the specified input into a schema version 
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>The schema version.</returns>
        public static SchemaVersion Parse(string input)
        {
            return new SchemaVersion(input.Substring(0,2), input.Substring(3,2), input.Substring(6,4));
        }
    }
}
