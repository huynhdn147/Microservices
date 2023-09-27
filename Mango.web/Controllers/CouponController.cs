using Mango.Web.Models;
using Mango.Web.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;

namespace Mango.Web.Controllers
{
    public class CouponController : Controller
    {
        private readonly ICouponservice _couponservice;
        public CouponController(ICouponservice couponservice)
        {
            _couponservice = couponservice;
        }

        public async Task<IActionResult> CouponIndex()
        {
            List<CouponDTO?> list = new();

            ResponseDto? Response = await _couponservice.GetAllCouponAsync();
            
            if(Response != null && Response.IsSuccess==true)
            {
                list = JsonConvert.DeserializeObject<List<CouponDTO>>(Convert.ToString(Response.Result));
            }
            else
            {
                TempData["error"] = Response?.Message;
            }    
            return View(list);
        }
        public async Task<IActionResult> CouponCreate()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CouponCreate(CouponDTO coupon)
        {
            if(ModelState.IsValid)
            {
                ResponseDto? Response = await _couponservice.CreateCouponAsync(coupon);

                if (Response != null && Response.IsSuccess == true)
                {
                    return RedirectToAction(nameof(CouponIndex)); 
                }
                else
                {
                    TempData["error"] = Response?.Message;
                }
            }
            return View(coupon);
        }
        public async Task<IActionResult> CouponDelete(int CouponId)
        {
			ResponseDto? Response = await _couponservice.GetCouponByIdAsync(CouponId);

			if (Response != null && Response.IsSuccess == true)
			{
				CouponDTO model = JsonConvert.DeserializeObject<CouponDTO>(Convert.ToString(Response.Result));
                return View(model);
			}
            else
            {
                TempData["error"] = Response?.Message;
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> CouponDelete(CouponDTO coupon)
        {
            ResponseDto? FindCoupon = await _couponservice.GetCouponByIdAsync(coupon.CouponId);

            if(FindCoupon != null && FindCoupon.IsSuccess == true)
            { 
                ResponseDto? Response = await _couponservice.DeleteCouponAsync(coupon.CouponId);

                if (Response != null && Response.IsSuccess == true)
                {
                    //CouponDTO model = JsonConvert.DeserializeObject<CouponDTO>(Convert.ToString(Response.Result));
                    return  RedirectToAction(nameof(CouponIndex)); ;
                }
                else
                {
                    TempData["error"] = Response?.Message;
                }
            }
            else
            {
                TempData["error"] = FindCoupon?.Message;
            }
            return View(FindCoupon);
        }
    }
}
