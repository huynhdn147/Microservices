﻿using AutoMapper;
using Mango.Services.ProductAPI.Data;
using Mango.Services.ProductAPI.Models;
using Mango.Services.ProductAPI.Models.Dto;
using Mango.Services.ProductAPI.Models.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.ProductAPI.Controllers
{
    [Route("api/product")]
    [ApiController]
    public class ProductAPIController : ControllerBase
    {
        private readonly AppDbContext _db;
        private ResponseDto _Response;
        private IMapper _mapper;
        public ProductAPIController(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _Response = new ResponseDto();
            _mapper = mapper;

        }
        [HttpGet]
        public ResponseDto Get() 
        {
            try
            {
                IEnumerable<Product> objList = _db.Products.ToList();
                _Response.Result = _mapper.Map<IEnumerable<ProductDto>>(objList);
                //return objList;
            }
            catch (Exception ex)
            {
                _Response.IsSuccess = false;
                _Response.Message = ex.Message;
            }
            return _Response;
        }
        [HttpGet]
        [Route("{id:int}")]
        public ResponseDto Get(int id)
        {
            try
            {
                Product obj = _db.Products.First(u => u.ProductId==id);
                _Response.Result= _mapper.Map<ProductDto>(obj); ;
            }
            catch (Exception ex)
            {
                _Response.Message= ex.Message;
                _Response.IsSuccess= false;
            }
            return _Response;
        }
        [HttpGet]
        [Route("GetByCode/{code}")]
        public ResponseDto GetByCode(int ProductId)
        {
            try
            {
                Product obj = _db.Products.First(u => u.ProductId == ProductId);
                if (obj == null)
                {
                    _Response.IsSuccess = false;
                }
                _Response.Result = _mapper.Map<ProductDto>(obj); 
            }
            catch (Exception ex)
            {
                _Response.Message = ex.Message;
                _Response.IsSuccess = false;
            }
            return _Response;
        }

        [HttpPost]
        [Authorize(Roles ="ADMIN")]
        public ResponseDto Post([FromBody] ProductDto ProductDTO)
        {
            try
            {
                Product obj = _mapper.Map<Product>(ProductDTO);
                _db.Products.Add(obj);
                _db.SaveChanges();
                _Response.Result=_mapper.Map<ProductDto>(obj);
            }
            catch (Exception ex)
            {
                _Response.Message = ex.Message;
                _Response.IsSuccess = false;
            }
            return _Response;
        }
        [HttpPut]
        [Authorize(Roles = "ADMIN")]
        public ResponseDto Put([FromBody] ProductDto ProductDTO)
        {
            try
            {
                Product obj = _mapper.Map<Product>(ProductDTO);
                _db.Products.Update(obj);
                _db.SaveChanges();
                _Response.Result = _mapper.Map<ProductDto>(obj);
            }
            catch (Exception ex)
            {
                _Response.Message = ex.Message;
                _Response.IsSuccess = false;
            }
            return _Response;
        }
        [HttpDelete]
        [Route("{id:int}")]
        [Authorize(Roles = "ADMIN")]
        public ResponseDto delete(int id)
        {
            try
            {
                Product obj = _db.Products.First(u=>u.ProductId ==id);
                _db.Products.Remove(obj);
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                _Response.Message = ex.Message;
                _Response.IsSuccess = false;
            }
            return _Response;
        }
    }
    
}
