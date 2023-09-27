using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utility;

namespace Mango.Web.Service
{
    public class Cartservice : ICartservice
    {
        private readonly IBaseService _baseService;
        public Cartservice(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDto?> ApplyCouponAsync(CartDto cartDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                Apitype = DS.Apitype.POST,
                Data = cartDto,
                Url = DS.ShoppingCartAPIBase + "/api/cart/ApplyCoupon"
            });
        }

        public async Task<ResponseDto?> EmailCart(CartDto cartDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                Apitype = DS.Apitype.POST,
                Data = cartDto,
                Url = DS.ShoppingCartAPIBase + "/api/cart/EmailCartRequest"
            });
        }

        public async Task<ResponseDto?> GetCartByUserId(string UserId)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                Apitype = DS.Apitype.GET,
                Url = DS.ShoppingCartAPIBase + "/api/cart/GetCart/" + UserId
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

        public async Task<ResponseDto?> RemoveFromCartAsync(int cartDetailsId)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                Apitype = DS.Apitype.POST,
                Data = cartDetailsId,
                Url = DS.ShoppingCartAPIBase + "/api/cart/RemoveCart"
            });
        }

        public async Task<ResponseDto?> UpsertCartAsync(CartDto cartDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                Apitype = DS.Apitype.POST,
                Data = cartDto,
                Url = DS.ShoppingCartAPIBase + "/api/cart/CartUpsert"
            });
        }
    }
}
