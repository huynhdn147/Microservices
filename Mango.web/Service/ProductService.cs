using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utility;

namespace Mango.Web.Service
{
    public class ProductService : IProductservice
    {
        private readonly IBaseService _baseService;
        public ProductService(IBaseService baseService)
        {
            _baseService = baseService;
        }
        public async Task<ResponseDto?> CreateProductAsync(ProductDto Product)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                Apitype = DS.Apitype.POST,
                Data = Product,
                Url = DS.ProductAPIBase + "/api/product/"
            });
        }

        public async Task<ResponseDto?> DeleteProductAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                Apitype = DS.Apitype.DELETE,
                Url = DS.ProductAPIBase + "/api/product/" + id
            });
        }

        public async Task<ResponseDto?> GetAllProductAsync()
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                Apitype = DS.Apitype.GET,
                Url = DS.ProductAPIBase + "/api/product"
            });
        }

        public async Task<ResponseDto?> GetProductAsync(string ProductCode)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                Apitype = DS.Apitype.GET,
                Url = DS.ProductAPIBase + "/api/product/GetByCode/" + ProductCode
            });
        }

        public async Task<ResponseDto?> GetProductByIdAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                Apitype = DS.Apitype.GET,
                Url = DS.ProductAPIBase + "/api/product/" + id
            });
        }

        public async Task<ResponseDto?> UpdateProductAsync(ProductDto Product)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                Apitype = DS.Apitype.PUT,
                Data = Product,
                Url = DS.ProductAPIBase + "/api/product/"
            });
        }
    }
}
