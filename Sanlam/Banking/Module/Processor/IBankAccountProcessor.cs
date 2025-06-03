using Sanlam.Banking.Module.Contract;

namespace Sanlam.Banking.Module.Processor
{

    public interface IBankAccountProcessor
    {
        public Task<WithdrawalResponse> WithdrawAsync(WithdrawalRequest request);
    }

    
}
