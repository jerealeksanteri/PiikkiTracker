using Microsoft.EntityFrameworkCore;
using PiikkiTracker.Data;
using PiikkiTracker.Repository.IRepository;

namespace PiikkiTracker.Repository
{
    public class UserProductRepository : IUserProductRepository
    {
        private readonly ApplicationDbContext _db;
        public UserProductRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<UserProduct> CreateUserProductAsync(UserProduct userProduct)
        {
            await _db.UserProducts.AddAsync(userProduct);
            await _db.SaveChangesAsync();
            return userProduct;
        }
        public async Task<bool> DeleteUserProductAsync(int userProductId)
        {
            var obj = _db.UserProducts.FirstOrDefaultAsync(u => u.Id == userProductId);
            if (await obj != null)
            {
                _db.UserProducts.Remove(await obj);
                return await _db.SaveChangesAsync() > 0;
            }
            return false;
        }
        public async Task<IEnumerable<UserProduct>> GetAllUserProductsAsync()
        {
            return await _db.UserProducts.ToListAsync();
        }
        public async Task<UserProduct> GetUserProductByIdAsync(int userProductId)
        {
            var obj = _db.UserProducts.FirstOrDefaultAsync(u => u.Id == userProductId);
            if (await obj == null)
            {
                return new UserProduct();
            }
            return await obj;
        }
        public async Task<UserProduct> UpdateUserProductAsync(UserProduct userProduct)
        {
            var obj = await _db.UserProducts.FirstOrDefaultAsync(u => u.Id == userProduct.Id);
            if (obj != null)
            {
                obj.UserId = userProduct.UserId;
                obj.ProductId = userProduct.ProductId;
                obj.Amount = userProduct.Amount;

                _db.UserProducts.Update(obj);
                await _db.SaveChangesAsync();
            }
            return obj;
        }
    }
}
