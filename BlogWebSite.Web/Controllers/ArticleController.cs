using BlogWebSite.Entity.Models.DTOs.Comments;
using BlogWebSite.Service.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace BlogWebSite.Web.Controllers
{
    public class ArticleController : Controller
    {
        #region Fields
        private readonly IArticleService _articleService;
        private readonly IMessageService _messageService;
        #endregion

        #region Ctor
        public ArticleController(IArticleService articleService, IMessageService messageService)
        {
            _articleService = articleService;
            _messageService = messageService;
        }
        #endregion

        public async Task<IActionResult> Index(Guid articleId)
        {
            throw new Exception("hata");
            var article = await _articleService
                .GetArticleWithCategoryNonDeletedAsync(articleId);
            ViewData["comments"] = await _messageService
                .GetMessagesByArticleAsync(articleId);
            return View(article);
        }

        [HttpPost]
        [Authorize]
        public async Task<JsonResult> AddMessage(string message, string articleId)
        {
            CommentDto comment = await _messageService
                .CreateMessageAsync(message, Guid.Parse(articleId));
            return Json(comment);
        }


    }
}
