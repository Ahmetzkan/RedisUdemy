using RedisExampleApp.API.Abstracts;
using RedisExampleApp.API.Entities;
using RedisExampleApp.Cache;
using StackExchange.Redis;
using System.Text.Json;

namespace RedisExampleApp.API.Concretes
{
    public class ProductRepositoryWithCacheDecorator : IProductRepository
    {
        private const string productKey = "productCaches";
        private readonly IProductRepository _productRepository;
        private readonly RedisService _redisService;
        private readonly IDatabase _cacheRepository;

        public ProductRepositoryWithCacheDecorator(IProductRepository productRepository, RedisService redisService)
        {
            _productRepository = productRepository;
            _redisService = redisService;
            _cacheRepository = redisService.GetDb(2);
        }

        public async Task<Product> AddAsync(Product product)
        {
            var newProduct = await _productRepository.AddAsync(product);
            if (await _cacheRepository.KeyExistsAsync(productKey))
            {
                await _cacheRepository.HashSetAsync(productKey, newProduct.Id, JsonSerializer.Serialize(newProduct));
            }
            return newProduct;
        }

        public async Task<List<Product>> GetAsync()
        {
            if (!await _cacheRepository.KeyExistsAsync(productKey)) return await LoadToCacheFromDbAsync();

            var products = new List<Product>();
            var productCaches = await _cacheRepository.HashGetAllAsync(productKey);
            foreach (var item in productCaches.ToList())
            {
                var product = JsonSerializer.Deserialize<Product>(item.Value);
                products.Add(product);
            }
            return products;
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            if (_cacheRepository.KeyExists(productKey))
            {
                var product = await _cacheRepository.HashGetAsync(productKey, id);
                return product.HasValue ? JsonSerializer.Deserialize<Product>(product) : null;
            }
            var products = await LoadToCacheFromDbAsync();

            return products.FirstOrDefault(x => x.Id == id);
        }

        private async Task<List<Product>> LoadToCacheFromDbAsync()
        {
            var products = await _productRepository.GetAsync();
            foreach (var item in products)
            {
                _cacheRepository.HashSetAsync(productKey, item.Id, JsonSerializer.Serialize(item));
            }
            return products;
        }
    }
}
