namespace Mango.Services.EmailAPI.Models.Dto
{
    public class CartDto
    {
        public CartHeaderdto CartHeader { get; set; }
        public IEnumerable<CartDetailDto>? cartDetails { get; set; }
    }
}
