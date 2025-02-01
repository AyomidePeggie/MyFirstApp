namespace MyFirstApp.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public int CategoryId { get; set; } //foreign key 
        public virtual Category Category { get; set; } //navigation property
    }
}
