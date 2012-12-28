using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using Parachute.Entities;
using Parachute.Utilities;

namespace Parachute.DataAccess
{
    public class SqlConnectionManager : IDisposable
    {
        private const string ApplicationName = "Parachute";
        private readonly SqlConnection _connection;
        public event EventHandler<SqlError> InfoOrErrorMessage;
        private static readonly Regex SplitterRegex = new Regex("^\\s*GO\\s*$", RegexOptions.IgnoreCase | RegexOptions.Multiline);

        #region Event Handling

        private void OnInfoOrErrorMessage(SqlError e)
        {
            var handler = InfoOrErrorMessage;
            if (handler != null) handler(this, e);
        }

        private void InfoMessageReceived(object sender, SqlInfoMessageEventArgs e)
        {
            foreach (SqlError error in e.Errors)
            {
                OnInfoOrErrorMessage(error);
            }
        }

        #endregion

        #region Constructor

        public SqlConnectionManager(string connectionString)
        {
            _connection = new SqlConnection(connectionString);
            if (_connection.State == ConnectionState.Broken || _connection.State == ConnectionState.Closed)
            {
                _connection.Open();
            }
            _connection.InfoMessage +=  InfoMessageReceived;
        }

        #endregion

        #region Manual Sql Executions

        private string GetSqlScriptFromFile(string sqlFile)
        {
            string sql;

            using (var strm = File.OpenRead(sqlFile))
            using (var reader = new StreamReader(strm))
            {
                sql = reader.ReadToEnd();
            }
            return sql;
        }

        public void ExecuteSchemaFile(string sqlFile, SchemaVersion version)
        {
            var sql = GetSqlScriptFromFile(sqlFile);
            var blocksOfSql = SplitterRegex.Split(sql);

            using (var transaction = _connection.BeginTransaction())
            {
                using (var cmd = _connection.CreateCommand())
                {
                    cmd.Connection = _connection;
                    cmd.Transaction = transaction;
                    cmd.CommandType = CommandType.Text;

                    foreach (var block in blocksOfSql.Where(block => block.Length > 0))
                    {
                        cmd.CommandText = block;

                        try
                        {
                            cmd.ExecuteNonQuery();
                        }
                        catch (SqlException sqlEx)
                        {
                            foreach (SqlError error in sqlEx.Errors)
                            {
                                OnInfoOrErrorMessage(error);
                            }

                            transaction.Rollback();
                            throw;
                        }
                        catch(Exception ex)
                        {
                            TraceHelper.Error(ex.Message);
                            throw;
                        }
                    }
                }



                using (var dc = new ParachuteModelDataContext(_connection))
                {
                    dc.Transaction = transaction;

                    var entry = new ParachuteSchemaChangeLog
                        {
                            MajorReleaseNumber = version.MajorVersion,
                            MinorReleaseNumber = version.MinorVersion,
                            PointReleaseNumber = version.PointRelease,
                            ScriptName = sqlFile
                        };

                    dc.ParachuteSchemaChangeLogs.InsertOnSubmit(entry);
                    try
                    {
                        dc.SubmitChanges();
                    }
                    catch (SqlException sqlEx)
                    {
                        foreach (SqlError error in sqlEx.Errors)
                        {
                            OnInfoOrErrorMessage(error);
                        }
                        transaction.Rollback();
                        throw;
                    }
                    catch (Exception ex)
                    {
                        TraceHelper.Error(ex.Message);
                        throw;
                    }
                }

                transaction.Commit();

            }

        }

        public void ExecuteScriptFile(string sqlFile)
        {
            var sql = GetSqlScriptFromFile(Path.GetFileName(sqlFile));
            var blocksOfSql = SplitterRegex.Split(sql);

            using (var transaction = _connection.BeginTransaction())
            {
                var transIsValid = true;
                using (var cmd = _connection.CreateCommand())
                {
                    cmd.Connection = _connection;
                    cmd.Transaction = transaction;
                    cmd.CommandType = CommandType.Text;

                    foreach (var block in blocksOfSql.Where(block => block.Length > 0))
                    {
                        cmd.CommandText = block;

                        try
                        {
                            cmd.ExecuteNonQuery();
                        }
                        catch (SqlException sqlEx)
                        {
                            foreach (SqlError error in sqlEx.Errors)
                            {
                                OnInfoOrErrorMessage(error);
                            }

                            transIsValid = false;
                            transaction.Rollback();
                            break;
                        }
                    }
                }

                if (transIsValid)
                {
                    transaction.Commit();
                }
            }
        }

        /// <summary>
        /// Sets up the database by creating the relevant parachute change tracking tables.
        /// </summary>
        /// <returns></returns>
        public bool SetupDatabase()
        {
            var result = true;

            using (var transaction = _connection.BeginTransaction())
            {
                var transIsValid = true;
                using (var cmd = _connection.CreateCommand())
                {
                    cmd.Connection = _connection;
                    cmd.Transaction = transaction;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = ResourceHelper.GetChangeLogCreationScript();

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (SqlException sqlEx)
                    {
                        foreach (SqlError error in sqlEx.Errors)
                        {
                            OnInfoOrErrorMessage(error);
                        }

                        result = transIsValid = false;
                        transaction.Rollback();
                    }
                }

                if (transIsValid)
                {
                    transaction.Commit();
                }
            }

            return result;
        }

        public bool IsDatabaseConfiguredForParachute()
        {
            try
            {
                using (var cmd = _connection.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = ResourceHelper.GetChangeLogExistsScript();
                    return ((int) cmd.ExecuteScalar() == 1);
                }
            }
            catch (SqlException sqlEx)
            {
                foreach (SqlError error in sqlEx.Errors)
                {
                    OnInfoOrErrorMessage(error);
                }
                return false;
            }
        }

        #endregion Manual Sql Executions


        #region IDisposable

        public void Dispose()
        {
            if (_connection != null)
            {
                _connection.Dispose();
            }
        }

        #endregion

        public SchemaVersion GetCurrentSchemaVersion()
        {
            using (var pmdc = new ParachuteModelDataContext(_connection))
            {
                var result = pmdc.ParachuteSchemaChangeLogs
                    .OrderByDescending(l => l.MajorReleaseNumber)
                    .ThenByDescending(l => l.MinorReleaseNumber)
                    .ThenByDescending(l => l.PointReleaseNumber)
                    .FirstOrDefault();

                return result == null
                    ? SchemaVersion.MinValue
                    : new SchemaVersion(result.MajorReleaseNumber, result.MinorReleaseNumber, result.PointReleaseNumber);
            }
        }

        #region Static Helper Methods
        
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
            catch(Exception ex)
            {
                TraceHelper.Error(ex.Message);
                TraceHelper.Error(ex.ToString());
                return string.Empty;
            }
        }

        public static bool TestConnection(string connectionString)
        {
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (var cmd = new SqlCommand("SELECT 1", conn))
                    {
                        return ((int) cmd.ExecuteScalar() == 1);
                    }
                }
            }
            catch(Exception ex)
            {
                TraceHelper.Error(ex.Message);
                TraceHelper.Error(ex.ToString());
            }

            return false;
        }

        #endregion

    }
}
