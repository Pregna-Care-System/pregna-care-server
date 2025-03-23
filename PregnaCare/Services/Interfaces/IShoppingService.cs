using PregnaCare.Core.DTOs;

namespace PregnaCare.Services.Interfaces
{
    public interface IShoppingService
    {
        Task<List<ProductDTO>> GetProductsAsync();
    }
}
    