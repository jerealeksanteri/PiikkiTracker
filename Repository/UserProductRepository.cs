using Microsoft.EntityFrameworkCore;
using PiikkiTracker.Data;
using PiikkiTracker.Repository.IRepository;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

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
            var obj = _db.UserProducts.Include(uj => uj.Product).Include(uj => uj.User).FirstOrDefaultAsync(u => u.Id == userProductId);
            if (await obj != null)
            {
                _db.UserProducts.Remove(await obj);
                return await _db.SaveChangesAsync() > 0;
            }
            return false;
        }
        public async Task<IEnumerable<UserProduct>> GetAllUserProductsAsync()
        {
            return await _db.UserProducts.Include(uj => uj.Product).Include(uj => uj.User).ToListAsync();
        }
        public async Task<UserProduct> GetUserProductByIdAsync(int userProductId)
        {
            var obj = _db.UserProducts.Include(uj => uj.Product).Include(uj => uj.User).FirstOrDefaultAsync(u => u.Id == userProductId);
            if (await obj == null)
            {
                return new UserProduct();
            }
            return await obj;
        }
        public async Task<UserProduct> UpdateUserProductAsync(UserProduct userProduct)
        {
            var obj = await _db.UserProducts.Include(uj => uj.Product).Include(uj => uj.User).FirstOrDefaultAsync(u => u.Id == userProduct.Id);
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


        public async Task<IEnumerable<UserProduct>> GetAllUserProductsByUserIdAsync(string userId)
        {
            return await _db.UserProducts.Include(uj => uj.Product).Include(uj => uj.User).Where(uj => uj.UserId == userId).ToListAsync();
        }

        public async Task<IEnumerable<UserProduct>> SearchUserProductsDynamically(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();
            string[] terms = searchTerm?.Trim().ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries);

            IQueryable<UserProduct> query = _db.UserProducts
                .Include(up => up.User)
                .Include(up => up.Product); 

            foreach (string term in terms)
            {
                // Check if the keyword is numeric (e.g., for Amount)
                if (int.TryParse(term, out int numericKeyword))
                {
                    query = query.Where(up => up.Amount == numericKeyword);
                }
                // Check if the keyword is a valid DateTime
                else if (DateTime.TryParse(term, out DateTime dateKeyword))
                {
                    query = query.Where(up => up.CreatedDate.Date == dateKeyword.Date); // Match dates exactly
                }
                else
                {
                    query = query.Where(up =>
                        (up.User.FirstName ?? "").ToLower().Contains(term) ||    // Match user first name
                        (up.User.LastName ?? "").ToLower().Contains(term) ||     // Match user last name
                        (up.Product.Name ?? "").ToLower().Contains(term)         // Match product name
                    );
                }

            }
            

            var results = await query.ToListAsync();
            Console.WriteLine($"Search Term: {searchTerm}, Results Count: {results.Count()}");

            return results;
        }
    }
}
