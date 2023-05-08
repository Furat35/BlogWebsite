using AutoMapper;
using BlogWebSite.Core.ResultMessages;
using BlogWebSite.Data.Repositories.Abstract;
using BlogWebSite.Data.UnitOfWorks;
using BlogWebSite.Entity.Entities.Concrete;
using BlogWebSite.Entity.Models.DTOs.Categories;
using BlogWebSite.Service.Extensions;
using BlogWebSite.Service.Helpers.ToastMessage;
using BlogWebSite.Service.Services.Abstract;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace BlogWebSite.Service.Services.Concrete
{
    public class CategoryService : ICategoryService
    {
        #region Fields
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IToastMsg _toast;
        private readonly ClaimsPrincipal _user;
        #endregion

        #region Properties
        public IRepository<Category> CategoryRepo => _unitOfWork.GetRepository<Category>();
        #endregion

        #region Ctor
        public CategoryService(IUnitOfWork unitWork, IMapper mapper, IHttpContextAccessor httpContextAccessor, IToastMsg toast)
        {
            _unitOfWork = unitWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _toast = toast;
            _user = _httpContextAccessor.HttpContext.User;
        }
        #endregion

        public async Task<List<CategoryDto>> GetAllCategoriesNonDeleteAsync()
        {
            var categories = await CategoryRepo
                .GetAllAsync(predicate: _ => !_.isDeleted);
            return _mapper
                .Map<List<CategoryDto>>(categories);
        }

        public async Task<List<CategoryDto>> GetFirstNCategoriesAsync(int count)
        {
            var categories = await CategoryRepo
                .TakeFirstNItemsAsync(count);
            return _mapper
                .Map<List<CategoryDto>>(categories);
        }

        public async Task<List<CategoryDto>> GetAllCategoriesDeletedAsync()
        {
            var categories = await CategoryRepo
                .GetAllAsync(predicate: _ => _.isDeleted);
            return _mapper.Map<List<CategoryDto>>(categories);
        }

        public async Task<Category> GetCategoryByGuidAsync(Guid id)
        {
            Category category = await CategoryRepo
                .GetByGuidAsync(id);
            return category;
        }

        #region Create
        public async Task CreateCategoryAsync(CategoryAddDto categoryAddDto)
        {
            var categoryExist = await CategoryRepo
                .GetAsync(_ => _.Name.Equals(categoryAddDto.Name));
            if (categoryExist != null)
            {
                if (categoryExist.isDeleted)
                    categoryExist.isDeleted = true;

                await ToastMessage(Messages.Category.Add(categoryExist.Name));
                return;
            }

            Category category = new(categoryAddDto.Name, _user.GetLoggedInEmail());
            await CategoryRepo.AddAsync(category);
            await ToastMessage(Messages.Category.Add(category.Name));
        }
        #endregion

        #region Update
        public async Task UpdateCategoryAsync(CategoryUpdateDto categoryUpdateDto)
        {
            string userEmail = _user.GetLoggedInEmail();
            Category category = await CategoryExistAsync(categoryUpdateDto.Id, false);
            if (category != null)
            {
                string categoryName = category.Name;
                category.Name = categoryUpdateDto.Name;
                category.ModifiedBy = userEmail;
                category.ModifiedDate = DateTime.Now;
                await CategoryRepo.UpdateAsync(category);
                await ToastMessage(Messages.Category.Update(categoryName));
            }
        }
        #endregion

        #region Delete & UndoDelete
        public async Task SafeDeleteCategoryAsync(Guid categoryId)
        {
            Category category = await CategoryExistAsync(categoryId, false);
            if (category != null && !category.isDeleted)
            {
                category.isDeleted = true;
                category.DeletedDate = DateTime.Now;
                category.DeletedBy = _user.GetLoggedInEmail();
                await CategoryRepo.UpdateAsync(category);
                await ToastMessage(Messages.Category.Delete(category.Name));
            }
        }

        public async Task UndoCategoryAsync(Guid categoryId)
        {
            Category category = await CategoryExistAsync(categoryId, true);
            if (category != null && category.isDeleted)
            {
                category.isDeleted = false;
                category.DeletedDate = null;
                category.DeletedBy = null;
                await CategoryRepo.UpdateAsync(category);
                await ToastMessage(Messages.Category.UndoDelete(category.Name));
            }
        }
        #endregion

        #region Private Methods
        private async Task<bool> ToastMessage(string message)
        {
            if (await SaveAsync())
            {
                _toast.Success(message);
                return true;
            }
            else
            {
                _toast.Error();
                return false;
            }
        }

        private async Task<bool> SaveAsync()
        {
            int effectedRows;
            try
            {
                effectedRows = await _unitOfWork.SaveAsync();
            }
            catch (Exception)
            {
                return false;
            }
            return effectedRows > 0
                ? true
                : false;
        }

        private async Task<Category?> CategoryExistAsync(Guid categoryId, bool isDeleted)
        {
            try
            {
                Category category = await CategoryRepo
                    .GetAsync(_ => _.Id == categoryId && _.isDeleted == isDeleted);
                return category;
            }
            catch
            {
                return null;
            }
        }
        #endregion
    }
}
