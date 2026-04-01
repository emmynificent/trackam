public interface ICategory
{
    Task <IEnumerable<Category>> GetAllCategory();
    Task <Category> CreateCategory (Category category);
    Task  DeleteCategory (Category category);
    Task <Category> UpdateCategory(Category category);
    Task<Category?> GetCategoryById(int Id);
    Task<Category> GetCategoryByName(string name);
    Task <IEnumerable<Category>> GetCategoriesByUserId(int userId);
    

}