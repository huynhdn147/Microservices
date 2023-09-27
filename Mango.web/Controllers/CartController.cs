using Mango.Web.Models;
using Mango.Web.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json.Serialization;

namespace Mango.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartservice _cartservice;
        public CartController(ICartservice cartservice)
        {
            _cartservice = cartservice;
        }

        public async Task<IActionResult> Remove(int cartDetailId)
        {
            var UserId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
            ResponseDto? response = await _cartservice.RemoveFromCartAsync(cartDetailId);
            if (response != null && response.IsSuccess == true)
            {
                TempData["success"] = "CartUpdate success";
                return RedirectToAction(nameof(CartIndex));
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ApplyCoupon(CartDto cartDto)
        {
            ResponseDto? response = await _cartservice.ApplyCouponAsync(cartDto);
            if (response != null && response.IsSuccess == true)
            {
                TempData["success"] = "CartUpdate success";
                return RedirectToAction(nameof(CartIndex));
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EmailCart(CartDto cartDto)
        {
            CartDto cart = await LoadCartDtoBaseOnLoggedInUser();

            cart.CartHeader.Email = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Email)?.FirstOrDefault()?.Value;


            ResponseDto? response = await _cartservice.EmailCart(cart);
            if (response != null && response.IsSuccess == true)
            {
                TempData["success"] = "Email will be process and sent shortly";
                return RedirectToAction(nameof(CartIndex));
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RemoveCoupon(CartDto cartDto)
        {
            cartDto.CartHeader.Couponcode = "";
            ResponseDto? response = await _cartservice.ApplyCouponAsync(cartDto);
            if (response != null && response.IsSuccess == true)
            {
                TempData["success"] = "CartUpdate success";
                return RedirectToAction(nameof(CartIndex));
            }
            return View();
        }

        public async Task<IActionResult> CartIndex()
        {
            return View(await LoadCartDtoBaseOnLoggedInUser());
        }
        private async Task<CartDto> LoadCartDtoBaseOnLoggedInUser()
        {
           var UserId = User.Claims.Where(u=>u.Type==JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
           ResponseDto? response = await _cartservice.GetCartByUserId(UserId);
            if (response != null && response.IsSuccess==true)
            {
                CartDto cartDto = JsonConvert.DeserializeObject<CartDto>(Convert.ToString(response.Result));
                return cartDto;
            }
            return new CartDto();
        }
    }
}
