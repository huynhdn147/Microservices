using AutoMapper;
using Azure;
using Mango.Services.CouponAPI.Data;
using Mango.Services.CouponAPI.Models;
using Mango.Services.CouponAPI.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.CouponAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CouponAPIController : ControllerBase
    {
        private readonly AppDbContext _db;
        private ResponseDto _Response;
        private IMapper _mapper;
        public CouponAPIController(AppDbContext db, IMapper mapper)
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
                IEnumerable<Coupon> objList = _db.coupons.ToList();
                _Response.Result = _mapper.Map<IEnumerable<CouponDTO>>(objList);
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
                Coupon obj = _db.coupons.First(u => u.CouponId==id);
                _Response.Result= _mapper.Map<CouponDTO>(obj); ;
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
        public ResponseDto GetByCode(string code)
        {
            try
            {
                Coupon obj = _db.coupons.First(u => u.CouponCode.ToUpper() == code.ToUpper());
                if (obj == null)
                {
                    _Response.IsSuccess = false;
                }
                _Response.Result = _mapper.Map<CouponDTO>(obj); 
            }
            catch (Exception ex)
            {
                _Response.Message = ex.Message;
                _Response.IsSuccess = false;
            }
            return _Response;
        }

        [HttpPost]
        public ResponseDto Post([FromBody] CouponDTO couponDTO)
        {
            try
            {
                Coupon obj = _mapper.Map<Coupon>(couponDTO);
                _db.coupons.Add(obj);
                _db.SaveChanges();
                _Response.Result=_mapper.Map<CouponDTO>(obj);
            }
            catch (Exception ex)
            {
                _Response.Message = ex.Message;
                _Response.IsSuccess = false;
            }
            return _Response;
        }
        [HttpPut]
        public ResponseDto Put([FromBody] CouponDTO couponDTO)
        {
            try
            {
                Coupon obj = _mapper.Map<Coupon>(couponDTO);
                _db.coupons.Update(obj);
                _db.SaveChanges();
                _Response.Result = _mapper.Map<CouponDTO>(obj);
            }
            catch (Exception ex)
            {
                _Response.Message = ex.Message;
                _Response.IsSuccess = false;
            }
            return _Response;
        }
        [HttpDelete]
        public ResponseDto delete(int id)
        {
            try
            {
                Coupon obj = _db.coupons.First(u=>u.CouponId ==id);
                _db.coupons.Remove(obj);
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
