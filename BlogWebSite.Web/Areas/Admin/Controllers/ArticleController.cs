using AutoMapper;
using BlogWebSite.Core.Const;
using BlogWebSite.Entity.Entities.Concrete;
using BlogWebSite.Entity.Models.DTOs.Articles;
using BlogWebSite.Service.Services.Abstract;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;

namespace BlogWebSite.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ArticleController : Controller
    {
        #region Fields
        private readonly IArticleService _articleService;
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        private readonly IValidator<Article> _articleValidator;
        private readonly IToastNotification _toast;
        #endregion

        #region Ctor
        public ArticleController(IArticleService articleService, ICategoryService categoryService, IMapper mapper,
            IValidator<Article> articleValidator, IToastNotification toast)
        {
            _articleService = articleService;
            _categoryService = categoryService;
            _mapper = mapper;
            _articleValidator = articleValidator;
            _toast = toast;
        }
        #endregion

        [Authorize(Roles = $"{RoleConst.SuperAdmin},{RoleConst.Admin}")]
        public async Task<IActionResult> Index()
        {
            var articles = await _articleService
                .GetAllArticlesWithCategoryNonDeletedAsync();
            return View(articles);
        }

        [Authorize(Roles = $"{RoleConst.SuperAdmin},{RoleConst.Admin}")]
        public async Task<IActionResult> DeletedArticle()
        {
            var articles = await _articleService
                .GetAllDeletedArticlesWithCategoryAsync();
            return View(articles);
        }

        #region Add
        [Authorize(Roles = $"{RoleConst.SuperAdmin},{RoleConst.Admin}")]
        public async Task<IActionResult> Add()
        {
            var categories = await _categoryService
                .GetAllCategoriesNonDeleteAsync();
            return View(new ArticleAddDto
            {
                Categories = categories
            });
        }

        [HttpPost]
        [Authorize(Roles = $"{RoleConst.SuperAdmin},{RoleConst.Admin}")]
        public async Task<IActionResult> Add(ArticleAddDto articleAddDto)
        {
            var map = _mapper.Map<Article>(articleAddDto);
            var result = await _articleValidator.ValidateAsync(map);
            if (articleAddDto.Photo is null)
                ModelState.AddModelError("", "Photo is required!");

            if (result.IsValid && ModelState.IsValid)
            {
                await _articleService
                    .CreateArticleAsync(articleAddDto);
                return RedirectToAction("Index", "Article", new { Area = "Admin" });
            }
            else
            {
                result.AddToModelState(ModelState);
                articleAddDto.Categories = await _categoryService
                    .GetAllCategoriesNonDeleteAsync();

                return View(articleAddDto);
            }
        }

        #endregion

        #region Update
        [Authorize(Roles = $"{RoleConst.SuperAdmin},{RoleConst.Admin}")]
        public async Task<IActionResult> Update(Guid articleId)
        {
            var article = await _articleService
                .GetArticleWithCategoryNonDeletedAsync(articleId);

            if (article is not null)
            {
                var articleUpdateDto = _mapper
                    .Map<ArticleUpdateDto>(article);
                articleUpdateDto.Categories = await _categoryService
                    .GetAllCategoriesNonDeleteAsync();

                return View(articleUpdateDto);
            }

            return NotFound();
        }

        [HttpPost]
        [Authorize(Roles = $"{RoleConst.SuperAdmin},{RoleConst.Admin}")]
        public async Task<IActionResult> Update(ArticleUpdateDto articleUpdateDto)
        {
            var map = _mapper
                .Map<Article>(articleUpdateDto);
            var result = await _articleValidator
                .ValidateAsync(map);

            if (result.IsValid && ModelState.IsValid)
            {
                await _articleService
                    .UpdateArticleAsync(articleUpdateDto);
                return RedirectToAction("Index", "Article", new { Area = "Admin" });
            }
            else
            {
                result.AddToModelState(ModelState);
                articleUpdateDto.Categories = await _categoryService
                    .GetAllCategoriesNonDeleteAsync();

                return View(articleUpdateDto);
            }
        }
        #endregion

        #region Delete & UndoDelete

        [Authorize(Roles = $"{RoleConst.SuperAdmin},{RoleConst.Admin}")]
        public async Task<IActionResult> Delete(Guid articleId)
        {
            await _articleService
                .SafeDeleteArticleAsync(articleId);
            return RedirectToAction("Index", "Article", new { Area = "Admin" });
        }

        [Authorize(Roles = $"{RoleConst.SuperAdmin},{RoleConst.Admin}")]
        public async Task<IActionResult> UndoDelete(Guid articleId)
        {
            await _articleService
                .UndoDeleteAsync(articleId);
            return RedirectToAction("DeletedArticle", "Article", new { Area = "Admin" });
        }
        #endregion
    }
}
