using Mango.Web.Models;

namespace Mango.Web.Service.IService
{
    public interface ICouponservice
    {
        Task<ResponseDto?> GetCouponAsync(string CouponCode);
        Task<ResponseDto?> GetAllCouponAsync();
        Task<ResponseDto?> GetCouponByIdAsync(int id);
        Task<ResponseDto?> CreateCouponAsync(CouponDTO coupon);
        Task<ResponseDto?> UpdateCouponAsync(CouponDTO coupon);
        Task<ResponseDto?> DeleteCouponAsync(int id);
    }
}
