using Mango.Web.Models;
using Mango.Web.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;

namespace Mango.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductservice _Productservice;
        public ProductController(IProductservice Productservice)
        {
            _Productservice = Productservice;
        }

        public async Task<IActionResult> ProductIndex()
        {
            List<ProductDto?> list = new();

            ResponseDto? Response = await _Productservice.GetAllProductAsync();
            
            if(Response != null && Response.IsSuccess==true)
            {
                list = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(Response.Result));
            }
            else
            {
                TempData["error"] = Response?.Message;
            }    
            return View(list);
        }
        public async Task<IActionResult> ProductCreate()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ProductCreate(ProductDto Product)
        {
            if(ModelState.IsValid)
            {
                ResponseDto? Response = await _Productservice.CreateProductAsync(Product);

                if (Response != null && Response.IsSuccess == true)
                {
                    return RedirectToAction(nameof(ProductIndex)); 
                }
                else
                {
                    TempData["error"] = Response?.Message;
                }
            }
            return View(Product);
        }
        public async Task<IActionResult> ProductDelete(int ProductId)
        {
			ResponseDto? Response = await _Productservice.GetProductByIdAsync(ProductId);

			if (Response != null && Response.IsSuccess == true)
			{
				ProductDto model = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(Response.Result));
                return View(model);
			}
            else
            {
                TempData["error"] = Response?.Message;
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> ProductDelete(ProductDto Product)
        {
            ResponseDto? FindProduct = await _Productservice.GetProductByIdAsync(Product.ProductId);

            if(FindProduct != null && FindProduct.IsSuccess == true)
            { 
                ResponseDto? Response = await _Productservice.DeleteProductAsync(Product.ProductId);

                if (Response != null && Response.IsSuccess == true)
                {
                    //ProductDTO model = JsonConvert.DeserializeObject<ProductDTO>(Convert.ToString(Response.Result));
                    TempData["success"] = "Product delete success";
                    return RedirectToAction(nameof(ProductIndex)); ;
                }
                else
                {
                    TempData["error"] = Response?.Message;
                }
            }
            else
            {
                TempData["error"] = FindProduct?.Message;
            }
            return View(FindProduct);
        }
        public async Task<IActionResult> ProductEdit(int ProductId)
        {
            ResponseDto? Response = await _Productservice.GetProductByIdAsync(ProductId);

            if (Response != null && Response.IsSuccess == true)
            {
                ProductDto model = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(Response.Result));
                return View(model);
            }
            else
            {
                TempData["error"] = Response?.Message;
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> ProductEdit(ProductDto Product)
        {
            ResponseDto? FindProduct = await _Productservice.GetProductByIdAsync(Product.ProductId);

            if (FindProduct != null && FindProduct.IsSuccess == true)
            {
                ResponseDto? Response = await _Productservice.UpdateProductAsync(Product);

                if (Response != null && Response.IsSuccess == true)
                {
                    //ProductDTO model = JsonConvert.DeserializeObject<ProductDTO>(Convert.ToString(Response.Result));
                    TempData["success"] = "Product update success";
                    return RedirectToAction(nameof(ProductIndex)); ;
                }
                else
                {
                    TempData["error"] = Response?.Message;
                }
            }
            else
            {
                TempData["error"] = FindProduct?.Message;
            }
            return View(FindProduct);
        }
    }
}
