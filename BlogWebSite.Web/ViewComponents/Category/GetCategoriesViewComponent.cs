using BlogWebSite.Service.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace BlogWebSite.Web.ViewComponents.Category
{
    public class GetCategoriesViewComponent : ViewComponent
    {
        private readonly ICategoryService _categoryService;

        public GetCategoriesViewComponent(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await _categoryService.GetFirstNCategoriesAsync(7);
            return View(categories);
        }
    }
}
