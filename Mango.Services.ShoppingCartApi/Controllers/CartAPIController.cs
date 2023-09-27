using AutoMapper;
using Azure;
using Mango.MessageBus;
using Mango.Services.ShoppingCartApi.Data;
using Mango.Services.ShoppingCartApi.Models;
using Mango.Services.ShoppingCartApi.Models.DTO;
using Mango.Services.ShoppingCartApi.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.ShoppingCartApi.Controllers
{
    [Route("api/cart")]
    [ApiController]
    public class CartAPIController : ControllerBase
    {
        private readonly AppDbContext _db;
        private ResponseDto _Response;
        private IMapper _mapper;
        private readonly IProductService _productService;
        private readonly ICouponService _couponService;
        private readonly IMessageBus _messageBus;
        private readonly  IConfiguration _configuration;
        public CartAPIController(AppDbContext db, IMapper mapper,
            IProductService productService, ICouponService couponService,IMessageBus messageBus,IConfiguration configuration)
        {
            _db = db;
            _Response = new ResponseDto();
            _mapper = mapper;
            _productService = productService;
            _couponService = couponService;
            _messageBus = messageBus;
            _configuration = configuration;
        }

        [HttpGet("GetCart/{userId}")]
        public async Task<ResponseDto> GetCart(string userId)
        {
            try
            {
                CartDto cart = new()
                {
                    CartHeader = _mapper.Map<CartHeaderdto>(await _db.cartHeaders.FirstOrDefaultAsync(u => u.UserId == userId))

                };
                IEnumerable<CartDetails> a = _db.CartDetails
                    .Where(u => u.CartHeaderId == cart.CartHeader.CartheaderId);


                cart.cartDetails = _mapper.Map<IEnumerable<CartDetailDto>>(_db.CartDetails
                    .Where(u => u.CartHeaderId == cart.CartHeader.CartheaderId));
                IEnumerable<ProductDto> productDtos = await _productService.GetProduct();

                foreach (var item in cart.cartDetails)
                {
                    item.Product = productDtos.FirstOrDefault(u=>u.ProductId==item.ProductId);
                    cart.CartHeader.CartTotal += (item.Count * item.Product.Price);
                }
                if(!string.IsNullOrEmpty(cart.CartHeader.Couponcode))
                {
                    CouponDTO coupon = await _couponService.GetCoupon(cart.CartHeader.Couponcode);
                    if (coupon.CouponCode != null && cart.CartHeader.CartTotal> coupon.MinAmount) { 
                        cart.CartHeader.CartTotal -= coupon.DiscountAmount;
                        cart.CartHeader.DisCount = coupon.DiscountAmount;

                    }
                }
                _Response.Result = cart;
            }
            catch (Exception ex)
            {
                _Response.IsSuccess = false;
                _Response.Message =ex.Message;
            }
            return _Response;
        }

        [HttpPost("RemoveCart")]
        public async Task<ResponseDto> RemoveCart([FromBody]int CartDetailId)
        {
            try
            {
                var cartDetail = await _db.CartDetails.AsNoTracking()
                    .FirstAsync(u => u.CartDetailsId == CartDetailId);
                int totalCountofCartItem = _db.CartDetails.Where(u => u.CartHeaderId == cartDetail.CartHeaderId).Count();
                _db.CartDetails.Remove(cartDetail);
                if(totalCountofCartItem == 1)
                {
                    var cardHeaderToRemove = await _db.cartHeaders.FirstOrDefaultAsync(u => u.CartheaderId
                    == cartDetail.CartHeaderId);
                    _db.cartHeaders.Remove(cardHeaderToRemove);
                }
                await _db.SaveChangesAsync();
                
                _Response.Result = true;
            }
            catch (Exception ex)
            {
                _Response.Message = ex.Message.ToString();
                _Response.IsSuccess = false;
            }
            return _Response;
        }

        [HttpPost("ApplyCoupon")]
        public async Task<ResponseDto> ApplyCoupon([FromBody] CartDto cartDto)
        {
            try
            {
                var cartFromDb = _db.cartHeaders.First(u => u.UserId == cartDto.CartHeader.UserId);
                cartFromDb.Couponcode = cartDto.CartHeader.Couponcode;
                _db.cartHeaders.Update(cartFromDb);
                await _db.SaveChangesAsync();
                _Response.Result = true;

            }
            catch (Exception ex)
            {
                _Response.Message = ex.Message.ToString();
                _Response.IsSuccess = false;
            }
            return _Response;
        }

        [HttpPost("EmailCartRequest")]
        public async Task<ResponseDto> EmailCartRequest([FromBody] CartDto cartDto)
        {
            try
            {
                await _messageBus.PublishMessage(cartDto, _configuration.GetValue<string>("TopicAndQueueNames:EmailShoppingCartQueue"));
                _Response.Result = true;

            }
            catch (Exception ex)
            {
                _Response.Message = ex.Message.ToString();
                _Response.IsSuccess = false;
            }
            return _Response;
        }

        [HttpPost("RemoveCoupon")]
        public async Task<ResponseDto> RemoveCoupon([FromBody] CartDto cartDto)
        {
            try
            {
                var cartFromDb = _db.cartHeaders.First(u => u.UserId == cartDto.CartHeader.UserId);
                cartFromDb.Couponcode = "";
                _db.cartHeaders.Update(cartFromDb);
                await _db.SaveChangesAsync();
                _Response.Result = true;

            }
            catch (Exception ex)
            {
                _Response.Message = ex.Message.ToString();
                _Response.IsSuccess = false;
            }
            return _Response;
        }

        [HttpPost("CartUpsert")]
        public async Task<ResponseDto> CartUpsert(CartDto cartDto)
        {
            try
            {
                var cartHeaderFromDb = await _db.cartHeaders.AsNoTracking()
                    .FirstOrDefaultAsync(u => u.UserId == cartDto.CartHeader.UserId);
                if (cartHeaderFromDb == null)
                {
                    //create header and details
                    CartHeader cartHeader = _mapper.Map<CartHeader>(cartDto.CartHeader);
                    _db.cartHeaders.Add(cartHeader);
                    await _db.SaveChangesAsync();
                    cartDto.cartDetails.First().CartHeaderId = cartHeader.CartheaderId;
                    _db.CartDetails.Add(_mapper.Map<CartDetails>(cartDto.cartDetails.First()));
                    await _db.SaveChangesAsync();
                }
                else
                {
                    //if header is not null
                    //check if details has same product
                    var cartDetailsFromDb = await _db.CartDetails.AsNoTracking().FirstOrDefaultAsync(
                        u => u.ProductId == cartDto.cartDetails.First().ProductId &&
                        u.CartHeaderId == cartHeaderFromDb.CartheaderId);
                    if (cartDetailsFromDb == null)
                    {
                        //create cartdetails
                        cartDto.cartDetails.First().CartHeaderId = cartHeaderFromDb.CartheaderId;
                        _db.CartDetails.Add(_mapper.Map<CartDetails>(cartDto.cartDetails.First()));
                        await _db.SaveChangesAsync();
                    }
                    else
                    {
                        //update count in cart details
                        cartDto.cartDetails.First().Count += cartDetailsFromDb.Count;
                        cartDto.cartDetails.First().CartHeaderId = cartDetailsFromDb.CartHeaderId;
                        cartDto.cartDetails.First().CartDetailsId = cartDetailsFromDb.CartDetailsId;
                        _db.CartDetails.Update(_mapper.Map<CartDetails>(cartDto.cartDetails.First()));
                        await _db.SaveChangesAsync();
                    }
                }
                _Response.Result = cartDto;
            }
            catch (Exception ex)
            {
                _Response.Message = ex.Message.ToString();
                _Response.IsSuccess = false;
            }
            return _Response;
        }

    }
}
