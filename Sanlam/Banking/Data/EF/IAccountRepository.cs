using System.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace Sanlam.Banking.Data.EF
{
    public interface IAccountRepository
    {
        Task<Account?> GetAccountAsync(long accountId);
        Task<IDbContextTransaction> AsTransactionAsync(IsolationLevel isolationLevel);
        Task<int> SaveChangesAsync();

    }
}
