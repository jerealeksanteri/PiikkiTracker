using System.Data;
using System.Xml;
using Microsoft.AspNetCore.Identity;
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
            
            
            // Debit the User
            ApplicationUser? user = await _db.Users.FirstOrDefaultAsync(u => u.Id == userProduct.UserId);
            if (user is null)
            {
                throw new DataException("UserId was not found!");
            }
            
            Product? product = await _db.Products.FirstOrDefaultAsync(p => p.Id == userProduct.ProductId);
            if (product is null)
            {
                throw new DataException("Product was not found!");
            }

            user.Debit(product.Price * userProduct.Amount);
            _db.Users.Update(user);

            await _db.SaveChangesAsync();

            return userProduct;
        }
        public async Task<bool> DeleteUserProductAsync(int userProductId)
        {
            var obj = await _db.UserProducts.Include(uj => uj.Product).Include(uj => uj.User).FirstOrDefaultAsync(u => u.Id == userProductId);
            if (obj != null)
            {
                // Credit the User
                ApplicationUser user = obj.User;
                user.Credit(obj.Amount * obj.Product.Price);

                _db.Users.Update(user);

                _db.UserProducts.Remove(obj);
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
            var obj = await _db.UserProducts.Include(up => up.Product).Include(up => up.User).FirstOrDefaultAsync(up => up.Id == userProduct.Id);
            if (obj != null)
            {
                // Check the balance
                Product? newProduct = await _db.Products.FirstOrDefaultAsync(p => p.Id == userProduct.ProductId);
                if (newProduct is null)
                {
                    throw new DataException("Product was not found");
                }
                bool isBalanceCorrect = (obj.Amount * obj.Product.Price) == (userProduct.Amount * newProduct.Price);

                // If balances do not match
                if (!isBalanceCorrect)
                {
                    ApplicationUser? debtor = await _db.Users.FirstOrDefaultAsync(u => u.Id == userProduct.UserId);
                    ApplicationUser? creditor = await _db.Users.FirstOrDefaultAsync(u => u.Id == obj.UserId);

                    if (debtor is null || creditor is null)
                    {
                        throw new DataException("Creditor or Debtor User was not found!");
                    }

                    creditor.Credit(obj.Amount * obj.Product.Price);
                    debtor.Debit(userProduct.Amount * newProduct.Price);
                    
                    _db.Users.Update(creditor);
                    _db.Users.Update(debtor);
                }


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
