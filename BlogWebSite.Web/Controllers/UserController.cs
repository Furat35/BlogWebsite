using AutoMapper;
using BlogWebSite.Core.Const;
using BlogWebSite.Entity.Entities.Concrete;
using BlogWebSite.Entity.Models.DTOs.Users;
using BlogWebSite.Service.Extensions;
using BlogWebSite.Service.Services.Abstract;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogWebSite.Web.Controllers
{
    [Authorize(Roles = RoleConst.User)]
    public class UserController : Controller
    {
        #region Fields
        private readonly IMapper _mapper;
        private readonly IValidator<AppUser> _validator;
        private readonly IValidator<UserAddDto> _userAddDtoValidator;
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly IValidator<UserProfileDto> _userProfileValidator;
        private readonly Guid _userId;
        #endregion

        #region Ctor
        public UserController(IMapper mapper, IValidator<AppUser> validator, IValidator<UserAddDto> userAddDtoValidator,
            IValidator<UserProfileDto> userProfileValidator, IUserService userService, IRoleService roleService, IHttpContextAccessor httpContext)
        {
            _mapper = mapper;
            _validator = validator;
            _userAddDtoValidator = userAddDtoValidator;
            _userService = userService;
            _roleService = roleService;
            _userProfileValidator = userProfileValidator;
            _userId = httpContext.HttpContext.User.GetLoggedInUserId();
        }
        #endregion

        #region Delete
        public async Task<IActionResult> Delete()
        {
            var result = await _userService
                .SafeDeleteUserAsync(_userId);
            if (result.identityResult == null)
                return NotFound();

            if (result.identityResult.Succeeded)
                return RedirectToAction("Index", "User", new { Area = "Admin" });
            else
            {
                result.identityResult.AddToIdentityModelState(ModelState);
                return View(_userId);
            }
        }
        #endregion

        #region Profile
        public async Task<IActionResult> Profile()
        {
            var profile = await _userService
                .GetUserProfileAsync();
            return View(profile);
        }

        [HttpPost]
        public async Task<IActionResult> Profile(UserProfileDto userProfileDto)
        {
            var isValid = await _userProfileValidator
                .ValidateAsync(userProfileDto);
            if (!isValid.IsValid)
                return View(
                    await _userService
                .GetUserProfileAsync());

            Guid userId = HttpContext.User.GetLoggedInUserId();
            var result = await _userService
                .UserProfileUpdateAsync(userProfileDto);
            return RedirectToAction(nameof(Profile));
        }
        #endregion
    }
}
