using Backend.Models;

namespace Backend.Services
{
    public class CategoryService
    {
        private readonly BmvContext _bmvContext = new BmvContext();
        public List<Category> GetAllCategories()
        {
            return _bmvContext.Categories.ToList();
        }
        public Category GetCategoryById(int id)
        {
            return _bmvContext.Categories.Find(id);
        }
        public bool AddCategory(Category category)
        {
            try
            {
                _bmvContext.Categories.Add(category);
                _bmvContext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool DeleteCategory(int id)
        {
            var res = _bmvContext.Categories.Find(id);
            if (res != null)
            {
                _bmvContext.Categories.Remove(res);
                _bmvContext.SaveChanges();
                return true;
            }
            return false;
        }
        public bool UpdateCategory(int id, Category category)
        {
            var res = _bmvContext.Categories.Find(id);
            if (res != null)
            {
                category.Name = res.Name;
                _bmvContext.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
