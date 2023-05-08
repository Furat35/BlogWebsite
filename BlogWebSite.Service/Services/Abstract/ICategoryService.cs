using BlogWebSite.Entity.Entities.Concrete;
using BlogWebSite.Entity.Models.DTOs.Categories;

namespace BlogWebSite.Service.Services.Abstract
{
    public interface ICategoryService
    {
        Task<List<CategoryDto>> GetAllCategoriesNonDeleteAsync();
        Task<List<CategoryDto>> GetAllCategoriesDeletedAsync();
        Task CreateCategoryAsync(CategoryAddDto categoryAddDto);
        Task UpdateCategoryAsync(CategoryUpdateDto categoryUpdateDto);
        Task<Category> GetCategoryByGuidAsync(Guid id);
        Task SafeDeleteCategoryAsync(Guid categoryId);
        Task UndoCategoryAsync(Guid categoryId);
        Task<List<CategoryDto>> GetFirstNCategoriesAsync(int count);

    }
}
