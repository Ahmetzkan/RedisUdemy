using Microsoft.EntityFrameworkCore;
using RedisExampleApp.API.Abstracts;
using RedisExampleApp.API.Context;
using RedisExampleApp.API.Entities;

namespace RedisExampleApp.API.Concretes
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _appDbContext;

        public ProductRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<Product> AddAsync(Product product)
        {
            await _appDbContext.Products.AddAsync(product);
            await _appDbContext.SaveChangesAsync();
            return product;
        }

        public async Task<List<Product>> GetAsync()
        {
            return await _appDbContext.Products.ToListAsync();
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            return await _appDbContext.Products.FindAsync(id);
        }
    }
}
