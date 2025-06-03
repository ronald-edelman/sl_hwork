using System.ComponentModel.DataAnnotations;

namespace Sanlam.Banking.Module.Contract
{
    public class WithdrawalRequest
    {
        [Required]
        public long AccountId { get; set; }

        [Required]
        public decimal Amount { get; set; }
    }
}
