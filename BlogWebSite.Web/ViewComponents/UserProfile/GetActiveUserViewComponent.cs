using BlogWebSite.Service.Extensions;
using BlogWebSite.Service.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace BlogWebSite.Web.ViewComponents.UserProfile
{
    public class GetActiveUserViewComponent : ViewComponent
    {
        private readonly IUserService _userService;

        public GetActiveUserViewComponent(IUserService userService)
        {
            _userService = userService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userId = HttpContext.User.GetLoggedInUserId();
            var user = await _userService.GetAppUserByIdAsync(userId);
            object userInfo = user.FirstName + " " + user.LastName;

            return View(userInfo);
        }
    }
}
