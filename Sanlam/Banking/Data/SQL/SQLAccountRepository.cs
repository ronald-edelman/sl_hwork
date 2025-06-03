using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Sanlam.Banking.Data.SQL
{
    //class added purely for demo reasons to indicate how to abstract SQL statements that are handcoded
    public class SQLAccountRepository : IAccountRepository
    {
        private readonly IDbConnection _connection;

        public SQLAccountRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<decimal?> GetAccountBalanceAsync(long accountId, IDbTransaction transaction)
        {
            const string sql = "SELECT balance FROM accounts WITH (XLOCK, ROWLOCK) WHERE id = @Id";
            return await _connection.QueryFirstOrDefaultAsync<decimal?>(sql, new { Id = accountId }, transaction);
        }

        public async Task<int> UpdateAccountBalanceAsync(long accountId, decimal amount, IDbTransaction transaction)
        {
            const string sql = "UPDATE accounts SET balance = balance - @Amount WHERE id = @Id";
            return await _connection.ExecuteAsync(sql, new { Amount = amount, Id = accountId }, transaction);
        }

    }
}
