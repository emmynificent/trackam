using Microsoft.EntityFrameworkCore;

public interface ICategoryRepository
{
    Task<Category> CreateCategory(Category category);
    Task DeleteCategory(int id);
    Task<Category> UpdateCategory(Category category);
}

public class CategoryRepository : ICategory
{
    private readonly TrackAmDbContext _dbContext;
    public CategoryRepository(TrackAmDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<Category> CreateCategory(Category category)
    {
        await _dbContext.Categories.AddAsync(category);
        await _dbContext.SaveChangesAsync();
        return category;
    }

    public async Task DeleteCategory(Category category)
    {
        _dbContext.Categories.Remove(category);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<Category> UpdateCategory(Category category)
    {
        _dbContext.Categories.Update(category);
        await _dbContext.SaveChangesAsync();
        return category;
    }
    
    public async Task<IEnumerable<Category>> GetAllCategory()
    {
        var category = await _dbContext.Categories
        .AsNoTracking()
        .ToListAsync();
        return category; 
    }

    public async Task<Category?> GetCategoryById(int Id)
    {
        var category = await _dbContext.Categories
        .AsNoTracking()
        .FirstOrDefaultAsync(c => c.Id == Id);
        
        return category;
    }

    public async Task<Category> GetCategoryByName(string name)
    {
        var category = await _dbContext.Categories
        .AsNoTracking()
        .FirstOrDefaultAsync(c => c.Name == name);
        
        return category;
    }

    public async Task<IEnumerable<Category>> GetCategoriesByUserId(int userId)
    {
        var categories = await _dbContext.Categories
        .Where(c => c.UserId == userId)
        .AsNoTracking()
        .ToListAsync();
        
        return categories;
    }
}