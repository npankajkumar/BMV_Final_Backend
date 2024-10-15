using Backend.Models;

namespace Backend.Services
{
    public interface INCategoryService
    {
        bool AddCategory(Category category);
        bool DeleteCategory(int id);
        List<Category> GetAllCategories();
        Category GetCategoryById(int id);
        bool UpdateCategory(int id, Category updatedCategory);
    }
}