using Mango.Web.Models;
using Mango.Web.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using Mango.Web.Service.IService;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using IdentityModel;

namespace Mango.web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IProductservice _Productservice;
        private readonly ICartservice _cartservice;

        public HomeController(ILogger<HomeController> logger, IProductservice Productservice,ICartservice cartservice)
        {
            _logger = logger;
            _Productservice = Productservice;
            _cartservice = cartservice;
        }
        
        public async Task<IActionResult> Index()
        {
            List<ProductDto?> list = new();

            ResponseDto? Response = await _Productservice.GetAllProductAsync();

            if (Response != null && Response.IsSuccess == true)
            {
                list = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(Response.Result));
            }
            else
            {
                TempData["error"] = Response?.Message;
            }
            return View(list);
        }

        [Authorize]
        public async Task<IActionResult> ProductDetails(int productId)
        {
            ProductDto? productDto = new();

            ResponseDto? Response = await _Productservice.GetProductByIdAsync(productId);

            if (Response != null && Response.IsSuccess == true)
            {
                productDto = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(Response.Result));
            }
            else
            {
                TempData["error"] = Response?.Message;
            }
            return View(productDto);
        }
        [Authorize]
        [HttpPost]
        [ActionName("ProductDetails")]
        public async Task<IActionResult> ProductDetails(ProductDto productDto)
        {
            CartDto cartDto = new CartDto()
            {
                CartHeader = new CartHeaderdto
                {
                    UserId = User.Claims.Where(u => u.Type == JwtClaimTypes.Subject)?.FirstOrDefault()?.Value
                }
            };

            CartDetailDto cartDetailDto = new CartDetailDto()
            {
                Count =productDto.Count,
                ProductId = productDto.ProductId
            };

            List<CartDetailDto> cartDetailDtos = new()
            {
                cartDetailDto
            };

            cartDto.cartDetails = cartDetailDtos;


            ResponseDto? Response = await _cartservice.UpsertCartAsync(cartDto);

            if (Response != null && Response.IsSuccess == true)
            {
                TempData["success"] = "Item has been add to cart";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["error"] = Response?.Message;
            }
            return View(productDto);
        }

        //[Authorize(Roles =DS.RoleAdmin)]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
