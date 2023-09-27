using Mango.Web.Models;

namespace Mango.Web.Service.IService
{
    public interface IProductservice
	{
        Task<ResponseDto?> GetProductAsync(string ProductCode);
        Task<ResponseDto?> GetAllProductAsync();
        Task<ResponseDto?> GetProductByIdAsync(int id);
        Task<ResponseDto?> CreateProductAsync(ProductDto Product);
        Task<ResponseDto?> UpdateProductAsync(ProductDto Product);
        Task<ResponseDto?> DeleteProductAsync(int id);
    }
}
