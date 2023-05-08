using AutoMapper;
using BlogWebSite.Core.Const;
using BlogWebSite.Entity.Entities.Concrete;
using BlogWebSite.Entity.Models.DTOs.Roles;
using BlogWebSite.Service.Services.Abstract;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogWebSite.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = $"{RoleConst.SuperAdmin}")]
    public class RoleController : Controller
    {
        #region Fields
        private readonly IRoleService _roleService;
        private readonly IMapper _mapper;
        private readonly IValidator<AppRole> _validator;
        #endregion

        #region Ctor
        public RoleController(IRoleService roleService, IMapper mapper, IValidator<AppRole> validator)
        {
            _roleService = roleService;
            _mapper = mapper;
            _validator = validator;
        }
        #endregion

        public async Task<IActionResult> Index()
        {
            var roles = await _roleService
                .GetAllRolesAsync(_ => !_.IsDeleted);
            return View(roles);
        }

        #region Add
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(RoleAddDto roleAddDto)
        {
            var map = _mapper.Map<AppRole>(roleAddDto);
            var result = await _validator.ValidateAsync(map);

            if (result.IsValid)
            {
                await _roleService
                    .CreateRoleAsync(roleAddDto);
                return RedirectToAction("Index", "Role", new { Area = "Admin" });
            }

            result.AddToModelState(ModelState);
            return View(roleAddDto);
        }
        #endregion

        #region Update
        public async Task<IActionResult> Update(Guid roleId)
        {
            var role = await _roleService
                .GetRoleByGuidAsync(roleId);
            if (role != null)
            {
                var map = _mapper.Map<RoleUpdateDto>(role);
                return View(map);
            }

            return RedirectToAction("Index", "Home", new { Area = "Admin" });
        }

        [HttpPost]
        public async Task<IActionResult> Update(RoleUpdateDto roleUpdateDto)
        {
            var map = _mapper.Map<AppRole>(roleUpdateDto);
            var result = await _validator.ValidateAsync(map);

            if (result.IsValid)
            {
                await _roleService
                    .UpdateRoleAsync(roleUpdateDto);
                return RedirectToAction("Index", "Role", new { Area = "Admin" });
            }

            result.AddToModelState(ModelState);
            return View();
        }
        #endregion

        #region Delete
        public async Task<IActionResult> Delete(Guid roleId)
        {
            await _roleService
                .SafeDeleteRoleAsync(roleId);
            return RedirectToAction("Index", "Role", new { Area = "Admin" });
        }
        #endregion
    }
}
