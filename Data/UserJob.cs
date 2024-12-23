using System.ComponentModel.DataAnnotations;

namespace PiikkiTracker.Data
{
    public class UserJob
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "User Is Required")]
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        [Required(ErrorMessage = "Job Is Required")]
        public int JobId { get; set; }

        public Job Job { get; set; }

        [Required(ErrorMessage = "Event Is Required")]
        public string Event { get; set; }

        public int? TransactionId { get; set; } = null;
        public Transaction Transaction { get; set; }

        public DateTime? TransactionAddedDate { get; set; }

        public string? Description { get; set; }

        public bool IsAccepted { get; set; } = false;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
