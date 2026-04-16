using System.ComponentModel.DataAnnotations;

namespace Day1.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set;}
       
        public int Quantity { get; set; }
        public Category? Category { get; set; }
        public int CategoryId { get; set; }
        public Supplier? Supplier { get; set; }
        public int SupplierId { get; set; }

    }
}
