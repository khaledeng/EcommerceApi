namespace Day1.DTOs
{
    public class ReadProductDto
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; }

        public int Quantity { get; set; }

        public string CategoryName { get; set; }
        public string SupplierName { get; set; }
    }
}
