namespace Mango.Services.ShoppingCartApi.Models.DTO
{
    public class CartDto
    {
        public CartHeaderdto CartHeader { get; set; }
        public IEnumerable<CartDetailDto>? cartDetails { get; set; }
    }
}
