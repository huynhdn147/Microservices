namespace Mango.Web.Models
{
    public class CartDto
    {
        public CartHeaderdto CartHeader { get; set; }
        public IEnumerable<CartDetailDto>? cartDetails { get; set; }
    }
}
