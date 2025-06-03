using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Sanlam.Banking.Data.EF
{

    public class EFAccountRepository : IAccountRepository
    {
        private readonly BankDbContext _context;

        public EFAccountRepository(BankDbContext context)
        {
            _context = context;
        }

        public async Task<Account?> GetAccountAsync(long accountId)
        {
            return await _context.Accounts.FirstOrDefaultAsync(a => a.Id == accountId);
        }


        public async Task<IDbContextTransaction> AsTransactionAsync(IsolationLevel isolationLevel)
        {
            var transaction = await _context.Database.BeginTransactionAsync(isolationLevel);
            return transaction;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }

}
