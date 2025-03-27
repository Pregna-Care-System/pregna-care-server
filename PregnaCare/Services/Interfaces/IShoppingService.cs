using PregnaCare.Core.DTOs;

namespace PregnaCare.Services.Interfaces
{
    public interface IShoppingService
    {
        Task<List<ProductDTO>> GetMilkProductsAsync();
        Task<List<ProductDTO>> GetBabyProductsAsync();
    }
}
