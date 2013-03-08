using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Parachute.Managers;
using IsolationLevel = System.Data.IsolationLevel;

namespace Parachute.DataAccess
{
    public enum ExecutionMode
    {
        /// <summary>
        /// Normal execution. All the changes are persistant if no Exception occurs.
        /// </summary>
        Normal,
        /// <summary>
        /// Test Mode. No changes is persistant. Transaction is rolled back.
        /// </summary>
        TestMode

    }
    /// <summary>
    /// 
    /// </summary>
    public class SingleTransactionCommand : IParachuteCommand
    {
        private readonly string _connectionString;
        private readonly TransactionScope _transactionScope;
        private readonly ParachuteContext _parachuteContext;

        private bool _allTransactionsSuccess = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="SingleTransactionCommand"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public SingleTransactionCommand(string connectionString)
            : this(connectionString, ExecutionMode.Normal)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SingleTransactionCommand"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="executionMode">The execution mode. </param>
        /// 
        public SingleTransactionCommand(string connectionString, ExecutionMode executionMode)
        {
            _connectionString = connectionString;
            _parachuteContext = new ParachuteContext(_connectionString);
            _transactionScope = new TransactionScope();
            _allTransactionsSuccess = false;
        }

        /// <summary>
        /// Executes the specified SQL scripts in a general single transaction
        /// </summary>
        /// <param name="sqlScripts">The SQL scripts.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public bool Execute(IEnumerable<string> sqlScripts)
        {
            var success = false;

            foreach (var block in sqlScripts.Where(block => block.Length > 0))
            {
                try
                {
                    _parachuteContext.Database.ExecuteSqlCommand(block);

                    success = true;

                }

                catch (Exception ex)
                {
                    _allTransactionsSuccess = false;
                    TraceHelper.Error(ex.Message);
                    throw;
                }
            }


            _allTransactionsSuccess &= success;

            return success;
        }




        public void Dispose()
        {

            if (_allTransactionsSuccess)
                _transactionScope.Complete();

            _transactionScope.Dispose();
            _parachuteContext.Dispose();


        }
    }
}
