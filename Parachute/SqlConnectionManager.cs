using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Parachute
{
    public class SqlConnectionManager : IDisposable
    {
        private const string ApplicationName = "Parachute";

        private readonly SqlConnection _connection;

        public event EventHandler<SqlError> InfoOrErrorMessage;

        private void OnInfoOrErrorMessage(SqlError e)
        {
            var handler = InfoOrErrorMessage;
            if (handler != null) handler(this, e);
        }

        public SqlConnectionManager(string connectionString)
        {
            _connection = new SqlConnection(connectionString);
            if (_connection.State == ConnectionState.Broken || _connection.State == ConnectionState.Closed)
            {
                _connection.Open();
            }
            _connection.InfoMessage +=  InfoMessageReceived;
        }

        private void InfoMessageReceived(object sender, SqlInfoMessageEventArgs e)
        {
            foreach (SqlError error in e.Errors)
            {
                OnInfoOrErrorMessage(error);
            }
        }


        public void ExecuteFile(string sqlFile)
        {
            string sql;

            using (var strm = File.OpenRead(sqlFile))
            using (var reader = new StreamReader(strm))
            {
                sql = reader.ReadToEnd();
            }

            var regex = new Regex("^\\s*GO\\s*$", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            var blocksOfSql = regex.Split(sql);

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

        public void Dispose()
        {
            if (_connection != null)
            {
                _connection.Dispose();
            }
        }

        #region Static Helper Methods
        
        public static string BuildConnectionString(string server, string database, string username, string password)
        {
            try
            {
                var scsb = new SqlConnectionStringBuilder();

                scsb.ApplicationName = ApplicationName;
                scsb.DataSource = server;
                scsb.InitialCatalog = database;
                scsb.UserID = username;
                scsb.Password = password;

                return scsb.ConnectionString;
            }
            catch(Exception ex)
            {
                Trace.WriteLineIf(Program.LoggingSwitch.TraceError, "===================================================================================");
                Trace.WriteLineIf(Program.LoggingSwitch.TraceError, ex.Message);
                Trace.WriteLineIf(Program.LoggingSwitch.TraceError, ex.ToString());
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
                Trace.WriteLineIf(Program.LoggingSwitch.TraceError, "===================================================================================");
                Trace.WriteLineIf(Program.LoggingSwitch.TraceError, ex.Message);
                Trace.WriteLineIf(Program.LoggingSwitch.TraceError, ex.ToString());
            }

            return false;
        }

        #endregion

    }
}
