using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utility;

namespace Mango.Web.Service
{
    public class Authservice : IAuthservice
    {
        private readonly IBaseService _baseService;
        public Authservice(IBaseService baseService)
        {
            _baseService = baseService;
        }
        public async Task<ResponseDto> AssignRoleAsync(RegisterationRequestDto requestDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                Apitype = DS.Apitype.POST,
                Data = requestDto,
                Url = DS.AuthAPIBase + "/api/auth/AssignRole"
            });
        }

        public async Task<ResponseDto> loginAsync(LoginRequestDto loginRequestDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                Apitype = DS.Apitype.POST,
                Data = loginRequestDto,
                Url = DS.AuthAPIBase + "/api/auth/login"
            }, withBearer: false);
        }

        public async Task<ResponseDto> RegisterAsync(RegisterationRequestDto requestDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                Apitype = DS.Apitype.POST,
                Data = requestDto,
                Url = DS.AuthAPIBase + "/api/auth/register"
            }, withBearer: false);
        }
    }
}
