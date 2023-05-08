using BlogWebSite.Core.Const;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogWebSite.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = $"{RoleConst.SuperAdmin}")]
    public class SmtpSettingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
