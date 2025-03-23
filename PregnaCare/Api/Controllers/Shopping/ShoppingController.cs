using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PregnaCare.Services.Interfaces;

namespace PregnaCare.Api.Controllers.Shopping
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ShoppingController : ControllerBase
    {
        private readonly IShoppingService _shoppingService;

        public ShoppingController(IShoppingService shoppingService)
        {
            _shoppingService = shoppingService;
        }

        [HttpGet("milk-products")]
        public async Task<IActionResult> GetMilkProducts()
        {
            var products = await _shoppingService.GetMilkProductsAsync();
            return Ok(products);
        }

        [HttpGet("baby-products")]
        public async Task<IActionResult> GetBabyProducts()
        {
            var products = await _shoppingService.GetBabyProductsAsync();
            return Ok(products);
        }
    }
}
