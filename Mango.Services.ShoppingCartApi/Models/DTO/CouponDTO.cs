namespace Mango.Services.ShoppingCartApi.Models.DTO
{
    public class CouponDTO
    {
        public int CouponId { set; get; }
        public string CouponCode { set; get; }
        public double DiscountAmount { set; get; }
        public int MinAmount { set; get; }
    }
}
