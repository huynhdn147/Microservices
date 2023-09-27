using Mango.Web.Models;

namespace Mango.Web.Service.IService
{
    public interface ICartservice
    {
        Task<ResponseDto?> GetCartByUserId(string UserId);
        Task<ResponseDto?> UpsertCartAsync(CartDto cartDto);
        Task<ResponseDto?> RemoveFromCartAsync(int cartDetailDto);
        Task<ResponseDto?> ApplyCouponAsync(CartDto cartDto);

        Task<ResponseDto?> EmailCart(CartDto cartDto);

    }
}
