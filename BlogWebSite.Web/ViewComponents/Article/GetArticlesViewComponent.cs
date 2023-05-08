using BlogWebSite.Service.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace BlogWebSite.Web.ViewComponents.Article
{
    public class GetArticlesViewComponent : ViewComponent
    {
        private readonly IArticleService _articleService;

        public GetArticlesViewComponent(IArticleService articleService)
        {
            _articleService = articleService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var articles = await _articleService.GetLastNPostsAsync(5);
            return View(articles);
        }
    }
}
