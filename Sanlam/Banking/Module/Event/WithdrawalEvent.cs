namespace Sanlam.Banking.Module.Event
{
    public class WithdrawalEvent: BankingEvent
    {
        public decimal Amount { get; private set; }
        public long AccountId { get; private set; }
        public string Status { get; private set; }

        private static string SUCCESSFUL = "SUCCESSFUL";
        private static string UNSUCCESSFUL = "UNSUCCESSFUL";

        public WithdrawalEvent(decimal amount, long accountId, bool successful)
        {
            Amount = amount;
            AccountId = accountId;
            Status = successful ? SUCCESSFUL : UNSUCCESSFUL;
        }

        public override string ToString()
        {
            return $"Withdrawal event for account {AccountId}";
        }

    }
}
