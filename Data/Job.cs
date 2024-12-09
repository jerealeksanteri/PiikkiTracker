
using System.ComponentModel.DataAnnotations;

namespace PiikkiTracker.Data
{
    public class Job
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Job name is required")]
        public string Name { get; set; }

        public string? Description { get; set; }

        [Required(ErrorMessage = "Payment is required")]
        public decimal Payment { get; set; }
        
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public ICollection<UserJob> UserJobs { get; set; } = new List<UserJob>();
    }
}
