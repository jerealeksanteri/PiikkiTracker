using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace PiikkiTracker.Data;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser
{
    public string? FirstName { get; set; } = null;
    public string? LastName { get; set; } = null;

    public string? TelegramNickname { get; set; } = null;

    private decimal Balance { get; set; } = 0;

    public ICollection<UserJob> UserJobs { get; set; } = new List<UserJob>();
    public ICollection<UserProduct> UserProducts { get; set; } = new List<UserProduct>();
    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

    public decimal GetBalance()
    {
        return this.Balance;
    }

    /// <summary>
    /// Decreases the Users Balance by the amount given
    /// </summary>
    /// <param name="amount">Amount of Balance taken</param>
    public void Debit(decimal amount)
    {
        this.Balance -= amount;
    }

    /// <summary>
    /// Increases the Users Balance by the amount given
    /// </summary>
    /// <param name="amount">Amount of balance given</param>
    public void Credit(decimal amount)
    {
        this.Balance += amount;
    }

}
