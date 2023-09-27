using Mango.Services.ShoppingCartApi.Models.DTO;

namespace Mango.Services.ShoppingCartApi.Services.IServices
{
    public interface IProductService
    {
        public Task<IEnumerable<ProductDto>> GetProduct();

    }
}
