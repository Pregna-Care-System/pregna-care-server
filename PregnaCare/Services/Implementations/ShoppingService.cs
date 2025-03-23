using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;
using PregnaCare.Core.DTOs;
using PregnaCare.Services.Interfaces;

namespace PregnaCare.Services.Implementations
{
    public class ShoppingService : IShoppingService
    {
        private readonly HttpClient _httpClient;

        public ShoppingService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<List<ProductDTO>> GetMilkProductsAsync()
        {
            return await GetProductsFromCategory("https://concung.com/sua-bot-101586.html");
        }
        public async Task<List<ProductDTO>> GetBabyProductsAsync()
        {
            return await GetProductsFromCategory("https://concung.com/do-dung-me-va-be-1011020.html");
        }
        private async Task<List<ProductDTO>> GetProductsFromCategory(string url)
        {
            var products = new List<ProductDTO>();
            try
            {
                string response = await _httpClient.GetStringAsync(url);
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(response);

                var productNodes = htmlDoc.DocumentNode.SelectNodes("//div[contains(@class, 'product-item mb-10 ')]");

                    foreach (var node in productNodes)
                    {
                        var nameNode = node.SelectSingleNode(".//a[contains(@class, 'line-clamp-2 font-14 product-name pointer')]");
                        string name = nameNode?.InnerText.Trim() ?? "Không có tên";

                        var priceNode = node.SelectSingleNode(".//span[contains(@class, 'product-price')]");
                        string price = priceNode?.InnerText.Trim() ?? "Không có giá";

                        string productUrl = nameNode?.GetAttributeValue("href", "#") ?? "#";
                        if (!productUrl.StartsWith("http")) productUrl = "https://concung.com" + productUrl;

                        var imgNode = node.SelectSingleNode(".//img[contains(@class, 'img-fluid')]");
                        string imageUrl = imgNode?.GetAttributeValue("data-src", "")
                                         ?? imgNode?.GetAttributeValue("src", "Không có ảnh")
                                         ?? "Không có ảnh";

                        products.Add(new ProductDTO
                        {
                            Name = name,
                            Price = price,
                            ImageUrl = imageUrl,
                            ProductUrl = productUrl
                        });

                        Console.WriteLine($"🛍 {name} - 💲 {price} - 🖼 {imageUrl} - 🔗 {productUrl}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Lỗi khi lấy dữ liệu từ {url}: {ex.Message}");
                }
            return products;
        }

    }
}
