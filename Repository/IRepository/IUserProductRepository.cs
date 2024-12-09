using PiikkiTracker.Data;

namespace PiikkiTracker.Repository.IRepository
{
    public interface IUserProductRepository
    {
        Task<IEnumerable<UserProduct>> GetAllUserProductsAsync();
        Task<UserProduct> GetUserProductByIdAsync(int userProductId);
        Task<UserProduct> CreateUserProductAsync(UserProduct userProduct);
        Task<UserProduct> UpdateUserProductAsync(UserProduct userProduct);
        Task<bool> DeleteUserProductAsync(int userProductId);
    }
}
