using Mango.Web.Models;

namespace Mango.Web.Service.IService
{
    public interface IAuthservice
    {
        Task<ResponseDto> loginAsync(LoginRequestDto loginRequestDto);
        Task<ResponseDto> RegisterAsync(RegisterationRequestDto requestDto);
        Task<ResponseDto> AssignRoleAsync(RegisterationRequestDto requestDto);
    }
}
