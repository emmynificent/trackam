using Microsoft.EntityFrameworkCore;
public class ExpenseRepository : IExpense
{
    private readonly TrackAmDbContext _dbContext;
    public ExpenseRepository(TrackAmDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<Expense> CreateExpenseAsync(Expense expense)
    {
        await _dbContext.Expenses.AddAsync(expense);
        await _dbContext.SaveChangesAsync();
        return expense;
    }
    public async Task <Expense> DeleteExpenseAsync(Expense expense)
    {
        _dbContext.Expenses.Remove(expense);
        await _dbContext.SaveChangesAsync(); 
        return expense; 

    }

    public async Task<IEnumerable<Expense>> GetAllExpensesAsyncByUser(int userId)
    {
        var expenses = await _dbContext.Expenses
        .AsNoTracking()
        .Where(x => x.UserId == userId).ToListAsync();
        return expenses ; 
    }

    public async Task<IEnumerable<Expense>> GetAllExpense()
    {
        var expenses = await _dbContext.Expenses
        .AsNoTracking()
        .ToListAsync();
        return expenses;
    }


    public async Task<Expense?> GetExpenseByIdAsync(int id)
    {
         var singleExpense = await _dbContext.Expenses.FindAsync(id);
         return singleExpense; 
    }

    public async Task<Expense> UpdateExpenseAsync(Expense expense)
    {
        _dbContext.Expenses.Update(expense);
        await _dbContext.SaveChangesAsync();
        return expense;
        
    }

    public async Task<IEnumerable<Expense>> GetExpensesByCategoryIdAsync(int categoryId, int userId)
    {
        var expenses = await _dbContext.Expenses
        .Where(x => x.CategoryId == categoryId && x.UserId == userId)
        .AsNoTracking()
        .ToListAsync();
        return expenses;
    }
}