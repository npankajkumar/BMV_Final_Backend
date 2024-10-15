using Backend.Models;
using Backend.Repositories;
using System.Collections.Generic;

namespace Backend.Services
{
    public class NCategoryService : INCategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public NCategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public List<Category> GetAllCategories()
        {
            return _categoryRepository.GetAllCategories().ToList();
        }

        public Category GetCategoryById(int id)
        {
            return _categoryRepository.GetCategoryById(id);
        }

        public bool AddCategory(Category category)
        {
            try
            {
                _categoryRepository.AddCategory(category);
                _categoryRepository.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteCategory(int id)
        {
            var category = _categoryRepository.GetCategoryById(id);
            if (category != null)
            {
                _categoryRepository.RemoveCategory(category);
                _categoryRepository.SaveChanges();
                return true;
            }
            return false;
        }

        public bool UpdateCategory(int id, Category updatedCategory)
        {
            var existingCategory = _categoryRepository.GetCategoryById(id);
            if (existingCategory != null)
            {
                existingCategory.Name = updatedCategory.Name;
                _categoryRepository.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
