using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace PiikkiTracker.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string TelegramNickname { get; set; }

        public ICollection<UserJob> UserJobs { get; set; } = new List<UserJob>();
        public ICollection<UserProduct> UserProducts { get; set; } = new List<UserProduct>();
    }

}