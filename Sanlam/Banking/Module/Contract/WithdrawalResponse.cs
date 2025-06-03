namespace Sanlam.Banking.Module.Contract
{

    public class WithdrawalResponse
    {

        public static WithdrawalResponse Success = WithdrawalResponse.For(BalanceUpdateResult.Success);
        public static WithdrawalResponse InsufficientFunds = WithdrawalResponse.For(BalanceUpdateResult.Success);
        public static WithdrawalResponse AccountNotFound = WithdrawalResponse.For(BalanceUpdateResult.Success);
        public static WithdrawalResponse Error = WithdrawalResponse.For(BalanceUpdateResult.Success);

        public BalanceUpdateResult UpdateResult { get; }

        public WithdrawalResponse(BalanceUpdateResult result)
        {
            UpdateResult = result;
        }

        public static WithdrawalResponse For(BalanceUpdateResult result)
        {
            return new WithdrawalResponse(result);
        }

    }


    public enum BalanceUpdateResult
    {
        Success,
        InsufficientFunds,
        AccountNotFound,
        Error
    }
}
