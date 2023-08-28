namespace Mango.Services.CouponAPI.Models.Dto
{
    public class CouponDTO
    {
        public int CouponId { set; get; }
        public string CouponCode { set; get; }
        public double DiscountAmount { set; get; }
        public int MinAmount { set; get; }
    }
}
