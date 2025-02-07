namespace MyFirstApp.Models
{
	public class ProductModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public decimal UnitPrice { get; set; }
		public int Quantity { get; set; }

		public int SelectedCategoryId {  get; set; }
	}
}
