using AutoMapper;
using BlogWebSite.Entity.Models.DTOs.Users;
using BlogWebSite.Service.Extensions;
using BlogWebSite.Service.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace BlogWebSite.Web.Areas.Admin.ViewComponents
{
    public class DashboardHeaderViewComponent : ViewComponent
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public DashboardHeaderViewComponent(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userId = HttpContext.User.GetLoggedInUserId();
            var loggedInUser = await _userService.GetAppUserByIdIncludeImageAsync(userId);

            var map = _mapper.Map<UserDto>(loggedInUser);

            var role = await _userService.GetUserRoleAsync(loggedInUser);
            map.Role = role;

            return View(map);
        }
    }
}
