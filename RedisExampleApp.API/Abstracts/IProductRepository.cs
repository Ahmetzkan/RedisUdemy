using RedisExampleApp.API.Entities;

namespace RedisExampleApp.API.Abstracts
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAsync();
        Task<Product> GetByIdAsync(int id);
        Task<Product> AddAsync(Product product);
    }
}
