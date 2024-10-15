using Backend.Models;

namespace Backend.Repositories
{
    public interface ICategoryRepository
    {
        void AddCategory(Category category);
        IQueryable<Category> GetAllCategories();
        Category GetCategoryById(int id);
        void RemoveCategory(Category category);
        void SaveChanges();
    }
}