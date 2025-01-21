using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace PiikkiTracker.Data
{
    public class UserProduct
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "User Is Required")]
        public string UserId { get; set; }

        [Required(ErrorMessage = "Product Is Required")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Amount Is Required")]
        public int Amount { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public ApplicationUser User { get; set; }
        public Product Product { get; set; }
    }
}
