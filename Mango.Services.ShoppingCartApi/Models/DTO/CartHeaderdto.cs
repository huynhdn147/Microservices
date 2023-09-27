using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mango.Services.ShoppingCartApi.Models.DTO
{
    public class CartHeaderdto
    {
        public int CartheaderId { get; set; }
        public string? UserId { get; set; }
        public string? Couponcode { get; set; }
        public double DisCount { get; set; }
        public double CartTotal { get; set; }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }

    }
}
