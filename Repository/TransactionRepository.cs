using System.Data;
using Microsoft.EntityFrameworkCore;
using PiikkiTracker.Data;
using PiikkiTracker.Repository.IRepository;

namespace PiikkiTracker.Repository;

public class TransactionRepository : ITransactionRepository
{
    private readonly ApplicationDbContext _db;

    public TransactionRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<Transaction>> GetAllTransactionsAsync()
    {
        return await _db.Transactions.ToListAsync();

    }


    public async Task<Transaction> GetTransactionByIdAsync(int transactionId)
    {
        var obj = await _db.Transactions.Include(t => t.User).FirstOrDefaultAsync(t => t.Id == transactionId);

        if (obj is null)
        {
            return new Transaction();
        }

        return obj;
    }


    public async Task<Transaction> CreateTransactionAsync(Transaction transaction)
    {
        await _db.Transactions.AddAsync(transaction);
        // Credit User
        ApplicationUser? user = await _db.Users.FirstOrDefaultAsync(u => u.Id == transaction.UserId);

        if (user is null)
        {
            throw new DataException("User was not found");
        }

        user.Credit(transaction.Amount);
        _db.Users.Update(user);

        await _db.SaveChangesAsync();
        return transaction;

    }


    public async Task<Transaction> UpdateTransactionAsync(Transaction transaction)
    {
        var obj = await _db.Transactions.Include(t => t.User).FirstOrDefaultAsync(t => t.Id == transaction.Id);
        
        if (obj != null)
        {   
            // Change the Balance
            ApplicationUser? debtor = await _db.Users.FirstOrDefaultAsync(u => u.Id == obj.UserId);
            ApplicationUser? creditor = await _db.Users.FirstOrDefaultAsync(u => u.Id == transaction.UserId);

            if ( debtor is null || creditor is null)
            {
                throw new DataException("Either Debtor or Creditor is null");
            }

            debtor.Debit(obj.Amount);
            creditor.Credit(obj.Amount);

            _db.Users.Update(creditor);
            _db.Users.Update(debtor);

            obj.Amount = transaction.Amount;
            obj.UserId = transaction.UserId;

            _db.Transactions.Update(obj);

            await _db.SaveChangesAsync();
        }

        
        return obj;
    }


    public async Task<bool> DeleteTransactionAsync(int transactionId)
    {
        var obj = await _db.Transactions.Include(t => t.User).FirstOrDefaultAsync(t => t.Id == transactionId);

        if (obj != null)
        {   
            // Debit the User
            ApplicationUser user = obj.User;
            user.Debit(obj.Amount);

            _db.Users.Update(user);
            _db.Transactions.Remove(obj);

            return await _db.SaveChangesAsync() > 0;
        }

        return false;
    }


    public async Task<IEnumerable<Transaction>> GetAllTransactionsByUserId(string userId)
    {
        return await _db.Transactions.Include(t => t.User).Where(t => t.UserId == userId).ToListAsync();
    }


    public async Task<IEnumerable<Transaction>> QueryTransactions(string searchTerm)
    {
        searchTerm = searchTerm.ToLower();
        string[] terms = searchTerm?.Trim().ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries);

        IQueryable<Transaction> query = _db.Transactions.Include(t => t.User);

        foreach (string term in terms)
        {
            // Check if the term is int
            if (int.TryParse(term, out int numericKeyword))
            {
                query = query.Where(t => t.Amount == numericKeyword);
            }
            // Check if the keyword is a valid Datetime
            else if (DateTime.TryParse(term, out DateTime dateKeyword))
            {
                query = query.Where(up => up.CreatedDate.Date == dateKeyword.Date); // Match dates exactly
            }
            else
            {
                query = query.Where(up =>
                    (up.User.FirstName ?? "").ToLower().Contains(term) ||    // Match user first name
                    (up.User.LastName ?? "").ToLower().Contains(term)     // Match user last name
                );
            }
        }

        return await query.ToListAsync();
    }
}
