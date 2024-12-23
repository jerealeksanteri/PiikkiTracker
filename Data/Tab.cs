using System.ComponentModel.DataAnnotations;

namespace PiikkiTracker.Data;

public class Tab
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Tab must belong to someone!")]
    public string UserId { get; set; }
    public ApplicationUser User { get; set; }
        
    public ICollection<UserProduct> UserProducts { get; set; } = new List<UserProduct>();
    public decimal TabTotal { get; private set; } = 0m;

    public int? TransactionId { get; set; } = null;
    public Transaction Transaction { get; set; }

    public DateTime TabOpened { get; set; } = DateTime.Now;
    public DateTime? TabClosed { get; set; } = DateTime.Now;
    public bool IsTabOpen { get; set; } = false;

    public DateTime CreatedDate { get; set; } = DateTime.Now;

    public void UpdateTabTotal()
    {
        TabTotal = UserProducts?.Sum(up => up.Amount * up.Product.Price) ?? 0m;
    }

}
