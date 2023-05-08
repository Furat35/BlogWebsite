using AutoMapper;
using BlogWebSite.Core.Const;
using BlogWebSite.Entity.Entities.Concrete;
using BlogWebSite.Entity.Models.DTOs.Users;
using BlogWebSite.Service.Extensions;
using BlogWebSite.Service.Services.Abstract;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogWebSite.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class UserController : Controller
    {
        #region Fields
        private readonly IMapper _mapper;
        private readonly IValidator<AppUser> _validator;
        private readonly IValidator<UserAddDto> _userAddDtoValidator;
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly IValidator<UserProfileDto> _userProfileValidator;
        #endregion

        #region Ctor
        public UserController(IMapper mapper, IValidator<AppUser> validator, IValidator<UserAddDto> userAddDtoValidator,
            IUserService userService, IRoleService roleService, IValidator<UserProfileDto> userProfileValidator)
        {
            _mapper = mapper;
            _validator = validator;
            _userAddDtoValidator = userAddDtoValidator;
            _userService = userService;
            _roleService = roleService;
            _userProfileValidator = userProfileValidator;
        }
        #endregion

        [Authorize(Roles = $"{RoleConst.SuperAdmin},{RoleConst.Admin}")]
        public async Task<IActionResult> Index()
        {
            var users = await _userService
                .GetAllUsersWithRoleAsync(_ => !_.IsDeleted);
            return View(users);
        }

        #region Add
        [Authorize(Roles = $"{RoleConst.SuperAdmin},{RoleConst.Admin}")]
        public async Task<IActionResult> Add()
        {
            var roles = await _roleService
                .GetAllRolesAsync(_ => !_.IsDeleted);
            return View(new UserAddDto { Roles = roles });
        }

        [HttpPost]
        [Authorize(Roles = $"{RoleConst.SuperAdmin},{RoleConst.Admin}")]
        public async Task<IActionResult> Add(UserAddDto userAddDto)
        {
            var result = await _userAddDtoValidator
                .ValidateAsync(userAddDto);

            if (result.IsValid)
            {
                var map = _mapper.Map<AppUser>(userAddDto);
                var createResult = await _userService
                    .CreateUserAsync(userAddDto);
                if (createResult.Succeeded)
                    return RedirectToAction("Index", "User", new { Area = "Admin" });
                else
                {
                    createResult.AddToIdentityModelState(ModelState);
                    return View(userAddDto);
                }
            }

            result.AddToModelState(ModelState);
            userAddDto.Roles = await _roleService
                .GetAllRolesAsync(_ => !_.IsDeleted);

            return View(userAddDto);
        }
        #endregion

        #region Update
        [Authorize(Roles = $"{RoleConst.SuperAdmin},{RoleConst.Admin}")]
        public async Task<IActionResult> Update(Guid userId)
        {
            var user = await _userService
                .GetAppUserByIdIncludeImageAsync(userId);
            if (user == null)
                return NotFound();

            var map = _mapper.Map<UserUpdateDto>(user);
            var userRoleId = await _roleService
                .GetRoleGuidAsync(await _userService.GetUserRoleAsync(user));

            if (userRoleId != null)
            {
                map.RoleId = Guid.Parse(userRoleId);
                map.Roles = await _roleService
                    .GetAllRolesAsync(_ => !_.IsDeleted);

                return View(map);
            }

            return RedirectToAction("Index", "User", new { Area = "Admin" });
        }

        [HttpPost]
        [Authorize(Roles = $"{RoleConst.SuperAdmin},{RoleConst.Admin}")]
        public async Task<IActionResult> Update(UserUpdateDto userUpdateDto)
        {
            var map = _mapper.Map<AppUser>(userUpdateDto);
            var result = await _validator
                .ValidateAsync(map);

            if (result.IsValid)
            {
                await _userService
                    .UpdateUserAsync(userUpdateDto);
                return RedirectToAction("Index", "User", new { Area = "Admin" });
            }

            return View(userUpdateDto);
        }
        #endregion

        #region Delete
        [Authorize(Roles = $"{RoleConst.SuperAdmin},{RoleConst.Admin}")]
        public async Task<IActionResult> Delete(Guid userId)
        {
            var result = await _userService
                .SafeDeleteUserAsync(userId);
            if (result.identityResult == null)
                return NotFound();

            if (result.identityResult.Succeeded)
                return RedirectToAction("Index", "User", new { Area = "Admin" });
            else
            {
                result.identityResult.AddToIdentityModelState(ModelState);
                return View(userId);
            }
        }
        #endregion

        #region Profile
        [Authorize(Roles = $"{RoleConst.SuperAdmin},{RoleConst.Admin}")]
        public async Task<IActionResult> Profile()
        {
            var profile = await _userService
                .GetUserProfileAsync();
            return View(profile);
        }

        [HttpPost]
        [Authorize(Roles = $"{RoleConst.SuperAdmin},{RoleConst.Admin}")]
        public async Task<IActionResult> Profile(UserProfileDto userProfileDto)
        {
            var isValid = await _userProfileValidator.ValidateAsync(userProfileDto);
            if (!isValid.IsValid)
                return View(
                    await _userService
                .GetUserProfileAsync());

            var result = await _userService
                .UserProfileUpdateAsync(userProfileDto);
            return result
                ? RedirectToAction("Index", "Home", new { Area = "Admin" })
                : View(userProfileDto);
        }
        #endregion

        #region Delete & UndoDelete
        public async Task<IActionResult> DeletedUser()
        {
            var users = await _userService
                .GetAllUsersWithRoleAsync(_ => _.IsDeleted);
            return View(users);
        }

        public async Task<IActionResult> UndoDelete(Guid userId)
        {
            var user = await _userService
                .UndoDeleteAsync(userId);
            return RedirectToAction("DeletedUser", "User", new { Area = "Admin" });
        }
        #endregion
    }
}
