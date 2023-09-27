using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Mango.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthservice _authservice;

        private readonly ITokenProvider _tokenProvider;
        public AuthController(IAuthservice authservice, ITokenProvider tokenProvider)
        {
            _authservice = authservice;
            _tokenProvider = tokenProvider;
        }
        [HttpGet]
        public IActionResult Login()
        {
            LoginRequestDto loginRequestDto = new LoginRequestDto();
            return View(loginRequestDto);
        }
        [HttpGet]
        public IActionResult Register()
        {
            var roleList = new List<SelectListItem>()
            {
                new SelectListItem {Text=DS.RoleAdmin,Value=DS.RoleAdmin},
                new SelectListItem {Text=DS.RoleCustomer,Value=DS.RoleCustomer}

            };
            ViewBag.RoleList = roleList;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDto request)
        {
            ResponseDto response = await _authservice.loginAsync(request);
            ResponseDto assignRole;
            if (response != null && response.IsSuccess == true)
            {
                LoginResponseDto loginResponseDto = 
                    JsonConvert.DeserializeObject<LoginResponseDto>(Convert.ToString(response.Result));
                await SigInUser(loginResponseDto);
                _tokenProvider.SetToken(loginResponseDto.Token);
                return RedirectToAction("Index", "Home");
                
            }
            else
            {
                TempData["error"] = response.Message;
            }
            //var roleList = new List<SelectListItem>()
            //{
            //    new SelectListItem {Text=DS.RoleAdmin,Value=DS.RoleAdmin},
            //    new SelectListItem {Text=DS.RoleCustomer,Value=DS.RoleCustomer}

            //};
            //ViewBag.RoleList = roleList;
            //TempData["error"] = response.Message;
            return View(request);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterationRequestDto request)
        {
            ResponseDto response = await _authservice.RegisterAsync(request);
            ResponseDto assignRole;
            if (response != null && response.IsSuccess==true) 
            {
                if(string.IsNullOrEmpty(request.Role))
                {
                    request.Role = DS.RoleCustomer;

                }
                else
                {
                    TempData["error"] = response.Message;
                }
                assignRole = await _authservice.AssignRoleAsync(request);
                if (assignRole != null && assignRole.IsSuccess==true) 
                {
                    TempData["success"] = "register is success";
                    return RedirectToAction(nameof(Login));
                }
                else
                {
                    TempData["error"] = response.Message;
                }
            }
            var roleList = new List<SelectListItem>()
            {
                new SelectListItem {Text=DS.RoleAdmin,Value=DS.RoleAdmin},
                new SelectListItem {Text=DS.RoleCustomer,Value=DS.RoleCustomer}

            };
            ViewBag.RoleList = roleList;
            TempData["error"] = response.Message;
            return View(request);
        }
        public async Task<IActionResult> logout()
        {
            await HttpContext.SignOutAsync();
            _tokenProvider.Cleartoken();
            return RedirectToAction("Index","Home"); ;
        }

        public async Task SigInUser(LoginResponseDto model)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(model.Token);
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Email,
                jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub,
                jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub).Value));
                identity.AddClaim(new Claim(JwtRegisteredClaimNames.Email,
                jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub).Value));

            identity.AddClaim(new Claim(ClaimTypes.Name,
                jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value));
            identity.AddClaim(new Claim(ClaimTypes.Role,
                jwt.Claims.FirstOrDefault(u => u.Type == "role").Value));

            var pricial = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, pricial);
        }
    }
}
