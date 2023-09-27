using System.ComponentModel.DataAnnotations;

namespace Mango.Web.Models
{
	public class ProductDto
	{
		public int ProductId { set; get; }
		public string Name { get; set; }
		public double Price { set; get; }
		public string Description { get; set; }
		public string CategoryName { get; set; }
		public string ImageUrl { get; set; }
		[Range(1,100)]
		public int Count { get; set; } = 1;
	}
}
