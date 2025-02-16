using IDistributedCacheRedisApp.Web.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Text;

namespace IDistributedCacheRedisApp.Web.Controllers
{
    public class ProductController : Controller
    {
        private IDistributedCache _distributedCache;
        public ProductController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }
        public async Task<IActionResult> Index()
        {
            DistributedCacheEntryOptions cacheEntryOptions = new DistributedCacheEntryOptions();
            cacheEntryOptions.AbsoluteExpiration = DateTime.Now.AddMinutes(10);

            Product product = new Product { Id = 1, Name = "Laptop", Price = 100 };
            string jsonProduct = JsonConvert.SerializeObject(product);
            Byte[] byteProduct = Encoding.UTF8.GetBytes(jsonProduct);

            _distributedCache.Set("product:1", byteProduct, cacheEntryOptions);
            //await _distributedCache.SetStringAsync("product:1", jsonProduct, cacheEntryOptions);
            return View();
        }

        public IActionResult Show()
        {
            Byte[] byteProduct = _distributedCache.Get("product:1");
            string jsonProduct = Encoding.UTF8.GetString(byteProduct);

            Product product = JsonConvert.DeserializeObject<Product>(jsonProduct);
            ViewBag.Product = product;
            return View();
        }

        public IActionResult Remove()
        {
            _distributedCache.Remove("name");
            return View();
        }

        public IActionResult ImageUrl()
        {
            byte[] imageBytes = _distributedCache.Get("image");
            return File(imageBytes, "image/png");
        }

        public IActionResult ImageCache()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/Code background.png");

            byte[] imageByte = System.IO.File.ReadAllBytes(path);
            _distributedCache.Set("image", imageByte);
            return View();
        }
    }
}
