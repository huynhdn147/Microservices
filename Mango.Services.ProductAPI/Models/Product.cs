using System.ComponentModel.DataAnnotations;

namespace Mango.Services.ProductAPI.Models
{
	public class Product
	{
		[Key]
		public int ProductId { set; get; }
		[Required]
		public string Name { get; set; }
		[Range(0, 1000)]
		public double Price { set; get; }
		public string Description { get; set; }
		public string CategoryName { get; set; }
		public string ImageUrl { get; set; }

	}
}
