using Backend.Models;
using System.Linq;

namespace Backend.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly BmvContext _context;

        public CategoryRepository(BmvContext context)
        {
            _context = context;
        }

        public IQueryable<Category> GetAllCategories()
        {
            return _context.Categories.AsQueryable();
        }

        public Category GetCategoryById(int id)
        {
            return _context.Categories.Find(id);
        }

        public void AddCategory(Category category)
        {
            _context.Categories.Add(category);
        }

        public void RemoveCategory(Category category)
        {
            _context.Categories.Remove(category);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
