using BlogWebSite.Service.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace BlogWebSite.Web.Controllers
{
    public class HomeController : Controller
    {
        #region Fields
        private readonly IArticleService _articleService;
        #endregion

        #region Ctor
        public HomeController(IArticleService articleService)
        {
            _articleService = articleService;
        }
        #endregion

        public IActionResult InvalidPage(string statusCode)
        {
            int code = int.Parse(statusCode);
            if (code >= 400 && code < 500)
            {
                if (code == 400)
                    ViewData["ErrorMessage"] = "İşlem sırasında hata oluştu. Tekrar deneyiniz!";
                if (code == 401)
                    ViewData["ErrorMessage"] = "Yetkisiz erişim!";
                if (code == 403)
                    ViewData["ErrorMessage"] = "Yetkisiz erişim!";
                if (code == 404)
                    ViewData["ErrorMessage"] = "Sayfa bulunamadı!";
            }
            else if (code >= 500 && code <= 599)
                ViewBag["ErrorMessage"] = "İşlem sırasında hata oluştu. Tekrar deneyiniz!";
            else
                ViewBag["ErrorMessage"] = "İşlem sırasında hata oluştu. Tekrar deneyiniz!!";

            return View();
        }

        public async Task<IActionResult> Index(Guid? categoryId, int currentPage = 1, int pageSize = 2, bool isAscending = false)
        {
            var articles = await _articleService
                .GetAllByPaggingAsync(categoryId, currentPage, pageSize, isAscending);
            return View(articles);
        }

        public async Task<IActionResult> Search(string keyword, int currentPage = 1, int pageSize = 2, bool isAscending = false)
        {
            var articles = await _articleService
                .SearchAsync(keyword, currentPage, pageSize, isAscending);
            return View(articles);
        }
    }
}