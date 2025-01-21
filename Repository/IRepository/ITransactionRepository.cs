using PiikkiTracker.Data;

namespace PiikkiTracker.Repository.IRepository
{
    public interface ITransactionRepository
    {
        Task<IEnumerable<Transaction>> GetAllTransactionsAsync();
        Task<Transaction> GetTransactionByIdAsync(int transactionId);
        Task<Transaction> CreateTransactionAsync(Transaction transaction);
        Task<Transaction> UpdateTransactionAsync(Transaction transaction);
        Task<bool> DeleteTransactionAsync(int transactionId);

        Task<IEnumerable<Transaction>> GetAllTransactionsByUserId(string userId);
        Task<IEnumerable<Transaction>> QueryTransactions(string searchTerm);
    }
}