public interface IExpense
{
    Task<IEnumerable<Expense>> GetAllExpensesAsyncByUser(int userId);
    Task<Expense?> GetExpenseByIdAsync(int id);
    Task<Expense> CreateExpenseAsync(Expense expense);
    Task<Expense> UpdateExpenseAsync(Expense expense);
    Task <Expense>DeleteExpenseAsync(Expense expense);
    Task <IEnumerable<Expense>> GetAllExpense ();
    Task<IEnumerable<Expense>> GetExpensesByCategoryIdAsync(int categoryId, int userId);
}