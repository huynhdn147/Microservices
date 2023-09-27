using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mango.Services.ShoppingCartApi.Models
{
    public class CartHeader
    {
        [Key]
        public int CartheaderId { get; set; }
        public string? UserId { get; set; }
        public string? Couponcode { get; set; }
        [NotMapped]
        public double DisCount { get; set; }
        [NotMapped]
        public double CartTotal { get; set;}
    }
}
