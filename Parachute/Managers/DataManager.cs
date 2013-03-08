using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

using Parachute.DataAccess;
using Parachute.Entities;
using Parachute.Exceptions;

namespace Parachute.Managers
{

    public class DataManager : IDisposable
    {
        private readonly string _connectionString;
        private readonly IParachuteCommand _parachuteCommand;

        private const string ApplicationName = "Parachute";



        //  private readonly SqlConnection _connection;
        public event EventHandler<SqlError> InfoOrErrorMessage;



        #region Event Handling

        /// <summary>
        /// Called when [info or error message] is raised by SqlConnection.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="parachuteCommand">The parachute command.</param>
        public DataManager(string connectionString, IParachuteCommand parachuteCommand)
        {
            _connectionString = connectionString;
            _parachuteCommand = parachuteCommand;
        }

        #endregion

        #region Manual Sql Executions



        /// <summary>
        /// Executes the schema file.
        /// Executes a number of sql blocks as scripts and subsequently logs the Schema File in the SchemaChangeLog
        /// All blocks are executed as a single transaction.
        /// </summary>
        /// <param name="sqlScripts">The SQL scripts.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="version">The version.</param>
        public void ExecuteSchemaFile(IEnumerable<string> sqlScripts, string fileName, SchemaVersion version)
        {
            var success = _parachuteCommand.Execute(sqlScripts);


            if (!success) return;

            using (var dc = new ParachuteContext(_connectionString))
            {
                var entry = new ParachuteSchemaChangeLog
                    {
                        MajorReleaseNumber = version.MajorVersion,
                        MinorReleaseNumber = version.MinorVersion,
                        PointReleaseNumber = version.PointRelease,
                        ScriptName = fileName
                    };

                dc.ParachuteSchemaChangeLogs.Add(entry);

                try
                {
                  
                    dc.SaveChanges();
                }
                catch (InvalidOperationException invalidOperationEx)
                {
                    TraceHelper.Error(invalidOperationEx.Message);
                    throw;
                }
                catch (Exception ex)
                {
                    TraceHelper.Error(ex.Message);
                    throw;
                }
            }
        }

        /// <summary>
        /// Executes the script file.
        /// </summary>
        /// <param name="sqlScripts">The SQL scripts.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="hash">The hash.</param>
        /// <param name="version">The version.</param>
        public void ExecuteScriptFile(IEnumerable<string> sqlScripts, string fileName, string hash, SchemaVersion version)
        {
            var success = _parachuteCommand.Execute(sqlScripts);
            if (!success) return;

            using (var dc = new ParachuteContext(_connectionString))
            {
                var entry = new ParachuteAppliedScriptsLog
                {
                    SchemaVersion = version.ToString(),
                    ScriptName = fileName,
                    DateApplied = DateTime.Now,
                    Hash = hash
                };

                dc.ParachuteAppliedScriptsLogs.Add(entry);
                try
                {
                    dc.SaveChanges();
                }
                catch (InvalidOperationException invalidOperationEx)
                {

                    TraceHelper.Error(invalidOperationEx.Message);
                    throw;
                }
                catch (Exception ex)
                {
                    TraceHelper.Error(ex.Message);
                    throw;
                }
            }


        }
        
        /// <summary>
        /// Configures the database.
        /// </summary>
        /// <param name="initDatabase">if set to <c>true</c> [init database].</param>
        /// <returns>The Current Schema Version of the Database</returns>
        /// <exception cref="ParachuteException">Aborting. Failed to setup database</exception>
        public SchemaVersion ConfigureDatabase(bool initDatabase)
        {
           return GetCurrentSchemaVersion();
        }

        #endregion Manual Sql Executions



        #region IDisposable

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _parachuteCommand.Dispose();
        }

        #endregion

        /// <summary>
        /// Gets the current schema version.
        /// </summary>
        /// <returns>The Current Schema Version of the Database</returns>
        public SchemaVersion GetCurrentSchemaVersion()
        {

            using (var pmdc = new ParachuteContext(_connectionString))
            {

                var result = pmdc.ParachuteSchemaChangeLogs
                    .OrderByDescending(l => l.MajorReleaseNumber)
                    .ThenByDescending(l => l.MinorReleaseNumber)
                    .ThenByDescending(l => l.PointReleaseNumber)
                    .FirstOrDefault();

                return result == null
                           ? SchemaVersion.MinValue
                           : new SchemaVersion(result.MajorReleaseNumber, result.MinorReleaseNumber,
                                               result.PointReleaseNumber);

            }
        }

        #region Static Helper Methods

        /// <summary>
        /// Builds the connection string.
        /// </summary>
        /// <param name="server">The server.</param>
        /// <param name="database">The database.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns>The Connecton String</returns>
        public static string BuildConnectionString(string server, string database, string username, string password)
        {
            try
            {
                var scsb = new SqlConnectionStringBuilder
                    {
                        ApplicationName = ApplicationName,
                        DataSource = server,
                        InitialCatalog = database,
                        UserID = username,
                        Password = password
                    };

                return scsb.ConnectionString;
            }
            catch (Exception ex)
            {
                TraceHelper.Error(ex.Message);
                TraceHelper.Error(ex.ToString());
                return string.Empty;
            }
        }

        /// <summary>
        /// Tests the connection.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <returns><c>true</c> if the connection test is successful</returns>
        public static bool TestConnection(string connectionString)
        {
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (var cmd = new SqlCommand("SELECT 1", conn))
                    {
                        return ((int)cmd.ExecuteScalar() == 1);
                    }
                }
            }
            catch (Exception ex)
            {
                TraceHelper.Error(ex.Message);
                TraceHelper.Error(ex.ToString());
            }

            return false;
        }

        #endregion

    }
}
