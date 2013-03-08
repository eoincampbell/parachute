using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Parachute.Managers;

namespace Parachute.DataAccess
{
    public class MultiTransactionCommand : IParachuteCommand
    {
        private readonly string _connectionString;

        public MultiTransactionCommand(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Executes the specified SQL scripts in a single transaction for each script.
        /// </summary>
        /// <param name="sqlScripts">The SQL scripts.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public bool Execute(IEnumerable<string> sqlScripts)
        {
            var success = false;

            using (var parachuteContext = new ParachuteContext(_connectionString))
            {
                using (var transaction = new TransactionScope())
                {
                    foreach (var block in sqlScripts.Where(block => block.Length > 0))
                    {
                        try
                        {
                            parachuteContext.Database.ExecuteSqlCommand(block);
                            success = true;
                        }
                        catch (Exception ex)
                        {
                            TraceHelper.Error(ex.Message);
                            throw;
                        }
                        transaction.Complete();
                    }
                }
            }
            return success;
        }


        public void Dispose()
        {
           
        }
    }
}
