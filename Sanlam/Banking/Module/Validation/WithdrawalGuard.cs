namespace Sanlam.Banking.Module.Validation
{
    internal class WithdrawalGuard
    {

        internal static void AgainstInvalidAccountId(long accountId, string paramName)
        {
            if (accountId < 1000000 || accountId > 50000000)
                throw new ArgumentException($"{paramName} is out of range");
        }

        internal static void AgainstLessThanZero(decimal value, string paramName)
        {
            if (value <= 0)
                throw new ArgumentOutOfRangeException($"{paramName} cannot be less than or equal to zero");
        }

    }
}
