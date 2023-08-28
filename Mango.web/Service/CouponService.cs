using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utility;

namespace Mango.Web.Service
{
    public class CouponService : ICouponservice
    {
        private readonly IBaseService _baseService;
        public CouponService(IBaseService baseService)
        {
            _baseService = baseService;
        }
        public async Task<ResponseDto?> CreateCouponAsync(CouponDTO coupon)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                Apitype = DS.Apitype.POST,
                Data = coupon,
                Url = DS.CouponAPIBase + "/api/coupon/"
            });
        }

        public async Task<ResponseDto?> DeleteCouponAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                Apitype = DS.Apitype.DELETE,
                Url = DS.CouponAPIBase + "/api/coupon/" + id
            });
        }

        public async Task<ResponseDto?> GetAllCouponAsync()
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                Apitype = DS.Apitype.GET,
                Url = DS.CouponAPIBase + "/api/coupon"
            });
        }

        public async Task<ResponseDto?> GetCouponAsync(string CouponCode)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                Apitype = DS.Apitype.GET,
                Url = DS.CouponAPIBase + "/api/coupon/GetByCode/"+CouponCode
            });
        }

        public async Task<ResponseDto?> GetCouponByIdAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                Apitype = DS.Apitype.GET,
                Url = DS.CouponAPIBase + "/api/coupon/" + id
            });
        }

        public async Task<ResponseDto?> UpdateCouponAsync(CouponDTO coupon)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                Apitype = DS.Apitype.PUT,
                Data = coupon,
                Url = DS.CouponAPIBase + "/api/coupon/"
            });
        }
    }
}
