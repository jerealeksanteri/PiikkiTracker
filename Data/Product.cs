using System.ComponentModel.DataAnnotations;

namespace PiikkiTracker.Data
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Product name is required")]
        public string Name { get; set; }
        
        [Required(ErrorMessage = "Price is required")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string? Description { get; set; }

        public byte[]? Icon { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public ICollection<UserProduct> UserProducts { get; set; } = new List<UserProduct>();

    }
}
