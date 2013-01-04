using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Parachute.Managers
{
// ReSharper disable InconsistentNaming
    public class FileIOManager
// ReSharper restore InconsistentNaming
    {
        /// <summary>
        /// Gets or sets the splitter.
        /// </summary>
        /// <value>
        /// The splitter.
        /// </value>
        private ISqlFileSplitter Splitter { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileIOManager" /> class.
        /// </summary>
        public FileIOManager() : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileIOManager" /> class.
        /// </summary>
        /// <param name="splitter">The splitter.</param>
        public FileIOManager(ISqlFileSplitter splitter)
        {
            Splitter = splitter;
        }

        /// <summary>
        /// Gets the SQL script from file.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>The contens of the SQL File</returns>
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

        /// <summary>
        /// Reads the SQL scripts.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>An enumerable of executable blocks of SQL based on the Splitter</returns>
        public IEnumerable<string> ReadSqlScripts(string path)
        {
            var fileContents = GetSqlScriptFromFile(path);

            return (Splitter != null)
                       ? Splitter.Split(fileContents)
                       : new List<string> {fileContents};
        }

        /// <summary>
        /// Gets the file MD5 hash of the file contents
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns>The MD5 Hash</returns>
        public string GetFileMD5Hash(string filePath)
        {
            using (var md5Hasher = MD5.Create())
            {
                using (var fs = File.OpenRead(filePath))
                {
                    var sb = new StringBuilder();

                    foreach (var byt in md5Hasher.ComputeHash(fs))
                    {
                        sb.Append(byt.ToString("x2").ToLower());
                    }

                    return sb.ToString();
                }
            }
        }
    }
}
