using RedisExampleApp.API.Entities;

namespace RedisExampleApp.API.Abstracts
{
    public interface IProductService
    {
        Task<List<Product>> GetAsync();
        Task<Product> GetByIdAsync(int id);
        Task<Product> AddAsync(Product product);
    }
}
