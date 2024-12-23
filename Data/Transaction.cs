using System.ComponentModel.DataAnnotations;

namespace PiikkiTracker.Data;

public class Transaction
{
    [Key]
    public int Id { get; set; }

    public ICollection<Tab> ClosedTabs { get; set; } = new List<Tab>();

    [Required(ErrorMessage = "Someone must have paid it??")]
    public string UserId { get; set; }
    public ApplicationUser User { get; set; }

    public ICollection<UserJob> UserJobs { get; set; } = new List<UserJob>();
    public decimal JobTotal { get; private set; } = 0m;

    public decimal TabsTotal { get; set; }

    public DateTime CreatedDate { get; set; }

    public void UpdateTotals()
    {
        TabsTotal = ClosedTabs?.Sum(t => t.TabTotal) ?? 0m;
        JobTotal = UserJobs?.Sum(uj => uj.Job.Payment) ?? 0m;
    }
}

