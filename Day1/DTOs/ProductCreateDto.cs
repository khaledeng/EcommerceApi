namespace Day1.DTOs
{
    public class ProductCreateDto
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; }

        public int Quantity { get; set; }


        public int CategoryId { get; set; }
        public int SupplierId { get; set; }
    }
}
