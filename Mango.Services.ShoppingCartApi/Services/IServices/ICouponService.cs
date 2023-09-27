using Mango.Services.ShoppingCartApi.Models.DTO;

namespace Mango.Services.ShoppingCartApi.Services.IServices
{
    public interface ICouponService
    {
         Task<CouponDTO> GetCoupon(string CouponCode);

    }
}
