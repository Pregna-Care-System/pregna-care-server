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

        public async Task<List<ProductDTO>> GetProductsAsync()
        {
            var url = "https://concung.com/sua-bot-101586.html";

            // Lấy nội dung HTML từ trang web
            var response = await _httpClient.GetStringAsync(url);

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(response);

            var products = new List<ProductDTO>();

            // XPath: Lấy danh sách sản phẩm
            var productNodes = htmlDoc.DocumentNode.SelectNodes("//div[contains(@class, 'product-item mb-10 ')]");

            if (productNodes == null)
            {
                Console.WriteLine("❌ Không tìm thấy sản phẩm! Kiểm tra lại XPath.");
                return products;
            }

            foreach (var node in productNodes)
            {
                // 📌 Lấy tên sản phẩm
                var nameNode = node.SelectSingleNode(".//a[contains(@class, 'line-clamp-2 font-14 product-name pointer')]");
                string name = nameNode?.InnerText.Trim() ?? "Không có tên";

                // 📌 Lấy giá sản phẩm
                var priceNode = node.SelectSingleNode(".//span[contains(@class, 'product-price  d-block')]");
                string price = priceNode?.InnerText.Trim() ?? "Không có giá";

                // 📌 Lấy URL sản phẩm
                string productUrl = nameNode?.GetAttributeValue("href", "#") ?? "#";
                if (!productUrl.StartsWith("http")) productUrl = "https://concung.com" + productUrl;

                // 📌 Lấy ảnh sản phẩm
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

                Console.WriteLine($"🛒 {name} - 💰 {price} - 🖼 {imageUrl} - 🔗 {productUrl}");
            }

            return products;
        }
    }
}
