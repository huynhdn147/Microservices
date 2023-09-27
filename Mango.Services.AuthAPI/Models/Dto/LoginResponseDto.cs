namespace Mango.Services.AuthAPI.Models.Dto
{
    public class LoginResponseDto
    {
        public UserDTO User { set; get; }
        public string Token { set; get; }
    }
}
