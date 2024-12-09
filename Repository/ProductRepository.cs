
using Microsoft.EntityFrameworkCore;
using PiikkiTracker.Data;
using PiikkiTracker.Repository.IRepository;

namespace PiikkiTracker.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _db;

        public ProductRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<Product> CreateProductAsync(Product product)
        {
            await _db.Products.AddAsync(product);
            await _db.SaveChangesAsync();
            return product;
        }

        public async Task<bool> DeleteProductAsync(int productId)
        {
            var obj = await _db.Products.FirstOrDefaultAsync(u => u.Id == productId);
            
            if (obj != null) {
                _db.Products.Remove(obj);
                return await _db.SaveChangesAsync() > 0;
            }
            return false;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _db.Products.ToListAsync();

        }

        public async Task<Product> GetProductByIdAsync(int productId)
        {
            var obj = await _db.Products.FirstOrDefaultAsync(u => u.Id == productId);
            
            if (obj == null)
            {
                return new Product();
            }
            return obj;
        }

        public async Task<Product> UpdateProductAsync(Product product)
        {
            var obj = await _db.Products.FirstOrDefaultAsync(u => u.Id == product.Id);
            
            if (obj != null)
            {
                obj.Name = product.Name;
                obj.Description = product.Description;
                obj.Price = product.Price;
                obj.Icon = product.Icon;

                _db.Products.Update(obj);
                await _db.SaveChangesAsync();
                return obj;
            }

            return product;
        }
    }
}
