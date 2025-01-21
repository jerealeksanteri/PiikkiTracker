using System.ComponentModel.DataAnnotations;

namespace PiikkiTracker.Data;

public class Transaction
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Someone must have paid it??")]
    public string UserId { get; set; }
    public ApplicationUser User { get; set; }

    [Required(ErrorMessage = "Type must be provided!")]
    public TransactionType TransactionType { get; set; } = TransactionType.CASH;

    [Required(ErrorMessage = "Amount must be specified!")]
    public decimal Amount { get; set; } = 0;

    public DateTime CreatedDate { get; set; }

}

public enum TransactionType {
    CASH,
    CARD
}
