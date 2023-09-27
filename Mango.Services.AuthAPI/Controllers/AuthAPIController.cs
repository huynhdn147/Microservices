using Mango.Services.AuthAPI.Models.Dto;
using Mango.Services.AuthAPI.Services;
using Mango.Services.AuthAPI.Services.IServices;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Mango.Services.AuthAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthAPIController : ControllerBase
    {
        private readonly IAuthService _authService;
        protected ResponseDto _responseDto;
        public AuthAPIController(IAuthService authService)
        {
            _authService = authService;
            _responseDto = new();
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterationRequestDto model)
        {
            var ErrMessage = await _authService.Register(model);
            if (!string.IsNullOrEmpty(ErrMessage))
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ErrMessage;
                return BadRequest(_responseDto);
            }
            return Ok(_responseDto);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequest)
        {
            var loginresponse = await _authService.Login(loginRequest);
            if (loginresponse.User == null)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "username or password is incorrect";
                return BadRequest(_responseDto);    
            }
            _responseDto.Result = loginresponse;
            return Ok(_responseDto);
        }
        [HttpPost("AssignRole")]
        public async Task<IActionResult> AssignRole([FromBody] RegisterationRequestDto model)
        {
            var AssignRoleSuccess = await _authService.AssignRole(model.Email,model.Role.ToUpper());
            if (!AssignRoleSuccess == null)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Error encounteder";
                return BadRequest(_responseDto);
            }
            return Ok(_responseDto);
        }
        
    }
}
