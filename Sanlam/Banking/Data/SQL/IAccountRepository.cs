using System.Data;

namespace Sanlam.Banking.Data.SQL
{
    public interface IAccountRepository
    {

        Task<decimal?> GetAccountBalanceAsync(long accountId, IDbTransaction transaction);
        Task<int> UpdateAccountBalanceAsync(long accountId, decimal amount, IDbTransaction transaction);
    }
}
