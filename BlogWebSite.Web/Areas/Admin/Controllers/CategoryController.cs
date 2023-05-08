using AutoMapper;
using BlogWebSite.Core.Const;
using BlogWebSite.Core.ResultMessages;
using BlogWebSite.Entity.Entities.Concrete;
using BlogWebSite.Entity.Models.DTOs.Categories;
using BlogWebSite.Service.Extensions;
using BlogWebSite.Service.Services.Abstract;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogWebSite.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = $"{RoleConst.SuperAdmin}")]
    public class CategoryController : Controller
    {
        #region Fields
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        private readonly IValidator<Category> _validator;
        #endregion

        #region Ctor
        public CategoryController(ICategoryService categoryService, IMapper mapper, IValidator<Category> validator)
        {
            _categoryService = categoryService;
            _mapper = mapper;
            _validator = validator;
        }
        #endregion

        public async Task<IActionResult> Index()
        {
            var categories = await _categoryService
                .GetAllCategoriesNonDeleteAsync();
            return View(categories);
        }

        public async Task<IActionResult> DeletedCategory()
        {
            var categories = await _categoryService
                .GetAllCategoriesDeletedAsync();
            return View(categories);
        }

        #region Add
        public IActionResult Add() => View();

        [HttpPost]
        public async Task<IActionResult> Add(CategoryAddDto categoryAddDto)
        {
            Category category = _mapper.Map<Category>(categoryAddDto);
            var result = await _validator
                .ValidateAsync(category);

            if (result.IsValid)
            {
                await _categoryService
                    .CreateCategoryAsync(categoryAddDto);
                return RedirectToAction("Index", "Category", new { Area = "Admin" });
            }

            result.AddToModelState(ModelState);
            return View(categoryAddDto);
        }

        [HttpPost]
        public async Task<IActionResult> AddWithAjax([FromBody] CategoryAddDto categoryAddDto)
        {
            Category category = _mapper
                .Map<Category>(categoryAddDto);
            var result = await _validator
                .ValidateAsync(category);

            if (result.IsValid)
            {
                await _categoryService.CreateCategoryAsync(categoryAddDto);
                return Json(Messages.Category.Add(categoryAddDto.Name));
            }

            return Json(result.Errors.First().ErrorMessage);
        }
        #endregion

        #region Update
        public async Task<IActionResult> Update(Guid categoryId)
        {
            var category = await _categoryService
                .GetCategoryByGuidAsync(categoryId);
            if (category != null)
            {
                var categoryUpdateDto = _mapper
                    .Map<CategoryUpdateDto>(category);
                return View(categoryUpdateDto);
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Update(CategoryUpdateDto categoryUpdateDto)
        {
            var map = _mapper.Map<Category>(categoryUpdateDto);
            var result = await _validator.ValidateAsync(map);
            if (result.IsValid)
            {
                await _categoryService
                    .UpdateCategoryAsync(categoryUpdateDto);
                return RedirectToAction("Index", "Category", new { Area = "Admin" });
            }

            result.AddToModelState(ModelState);
            return View(categoryUpdateDto);
        }
        #endregion

        #region Delete & UndoDelete
        public async Task<IActionResult> Delete(Guid categoryId)
        {
            await _categoryService
                .SafeDeleteCategoryAsync(categoryId);
            return RedirectToAction("Index", "Category", new { Area = "Admin" });
        }

        public async Task<IActionResult> UndoDelete(Guid categoryId)
        {
            await _categoryService.UndoCategoryAsync(categoryId);
            return RedirectToAction("DeletedCategory", "Category", new { Area = "Admin" });
        }
        #endregion
    }
}
