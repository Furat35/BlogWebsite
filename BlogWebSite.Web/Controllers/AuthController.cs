using AutoMapper;
using BlogWebSite.Core.Const;
using BlogWebSite.Core.ResultMessages;
using BlogWebSite.Entity.Entities.Concrete;
using BlogWebSite.Entity.Models.DTOs.Users;
using BlogWebSite.Service.Extensions;
using BlogWebSite.Service.Helpers.Email;
using BlogWebSite.Service.Services.Abstract;
using BlogWebSite.Web.Models;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace BlogWebSite.Web.Controllers
{
    public class AuthController : Controller
    {
        #region Fields
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IValidator<AppUser> _validator;
        private readonly IToastNotification _toast;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<AppUser> _userManager;
        private readonly ClaimsPrincipal _user;
        #endregion

        #region Ctor
        public AuthController(SignInManager<AppUser> signInManager, IUserService userService, IMapper mapper, IValidator<AppUser> validator,
            IToastNotification toast, IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager)
        {
            _signInManager = signInManager;
            _userService = userService;
            _mapper = mapper;
            _validator = validator;
            _toast = toast;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _user = _httpContextAccessor.HttpContext.User;
        }
        #endregion

        #region Login
        public IActionResult Login() => User.Identity != null && !User.Identity.IsAuthenticated
                ? View()
                : RedirectToAction("Index", "Home", new { Area = "" });

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UserLoginDto userLoginDto)
        {
            if (User.Identity != null && !User.Identity.IsAuthenticated)
            {
                var user = await _userService
                    .GetAppUserByEmailAsync(userLoginDto.Email != null ? userLoginDto.Email : string.Empty);

                if (user != null)
                {
                    var result = await _signInManager
                        .PasswordSignInAsync(user, userLoginDto.Password == null ? "" : userLoginDto.Password, userLoginDto.RememberMe, false);
                    if (!result.Succeeded)
                        return RedirectToAction(nameof(Login));

                    return RedirectToAction("Index", "Home", new { Area = "" });
                }

                ModelState.AddModelError("", "E-post adresiniz veya şifreniz yanlıştır.");
                return View(userLoginDto);
            }
            else
                return RedirectToAction("Index", "Home", new { Area = "" });
        }
        #endregion

        #region Logout
        [HttpGet]
        [Authorize(Roles = $"{RoleConst.SuperAdmin},{RoleConst.Admin},{RoleConst.User}")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            HttpContext.Session.Clear();

            return RedirectToAction("Index", "Home", new { Area = "" });
        }
        #endregion

        #region Register
        [AllowAnonymous]
        public IActionResult Register() => User.Identity != null && !User.Identity.IsAuthenticated
               ? View()
               : RedirectToAction("Index", "Home", new { Area = "" });

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterDto userRegisterDto)
        {
            var user = await _userService
                .GetAppUserByEmailAsync(userRegisterDto.Email ?? string.Empty);
            if (user != null)
                ModelState.AddModelError("", "Farklı bir email adresi ile üye olunuz.");
            if (userRegisterDto.Password == null)
                ModelState.AddModelError("Password", "Bu alanı boş bırakmayınız.");

            user = _mapper.Map<AppUser>(userRegisterDto);
            var result = await _validator.ValidateAsync(user);

            if (result.IsValid)
            {
                var map = _mapper.Map<UserAddDto>(userRegisterDto);
                var createResult = await _userService
                    .CreateUserAsync(map);

                if (createResult.Succeeded)
                {
                    _toast.AddSuccessToastMessage("Başarıyla kayıt olundu.", new ToastrOptions { Title = Messages.ToastTitle.Success });
                    return RedirectToAction("Index", "Home", new { Area = "" });
                }
                else
                {
                    createResult.AddToIdentityModelState(ModelState);
                    return View(userRegisterDto);
                }
            }

            result.AddToModelState(ModelState);
            return View(userRegisterDto);
        }
        #endregion


        #region Password Reset With Email
        public async Task<IActionResult> ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword([Required] string email)
        {
            if (!ModelState.IsValid)
                return View(email);

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return RedirectToAction(nameof(ForgotPasswordConfirmation));

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var link = Url.Action("ResetPassword", "Auth", new { token, email = user.Email }, Request.Scheme);

            EmailHelper emailHelper = new EmailHelper();
            bool emailResponse = emailHelper.SendEmailPasswordReset(user.Email, link);

            if (emailResponse)
                return RedirectToAction(nameof(ForgotPasswordConfirmation));
            else
            {
                // log email failed 
            }
            return View(email);
        }

        public async Task<IActionResult> ForgotPasswordConfirmation()
        {
            return View();
        }

        public IActionResult ResetPassword(string token, string email)
        {
            var model = new ResetPassword { Token = token, Email = email };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPassword resetPassword)
        {
            if (!ModelState.IsValid)
                return View(resetPassword);

            var user = await _userManager.FindByEmailAsync(resetPassword.Email);
            if (user == null)
                RedirectToAction(nameof(ResetPasswordConfirmation));

            var resetPassResult = await _userManager.ResetPasswordAsync(user, resetPassword.Token, resetPassword.Password);
            if (!resetPassResult.Succeeded)
            {
                foreach (var error in resetPassResult.Errors)
                    ModelState.AddModelError(error.Code, error.Description);
                return View(resetPassword);
            }

            return RedirectToAction(nameof(ResetPasswordConfirmation));
        }

        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }
        #endregion

        public IActionResult AccessDenied(string ReturnUrl) => View();
    }
}
