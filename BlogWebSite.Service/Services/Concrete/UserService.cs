using AutoMapper;
using BlogWebSite.Core.Const;
using BlogWebSite.Core.ResultMessages;
using BlogWebSite.Data.Repositories.Abstract;
using BlogWebSite.Data.UnitOfWorks;
using BlogWebSite.Entity.Entities.Concrete;
using BlogWebSite.Entity.Enums;
using BlogWebSite.Entity.Models.DTOs.Users;
using BlogWebSite.Service.Extensions;
using BlogWebSite.Service.Helpers.Images;
using BlogWebSite.Service.Helpers.ToastMessage;
using BlogWebSite.Service.Services.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Security.Claims;

namespace BlogWebSite.Service.Services.Concrete
{
    public class UserService : IUserService
    {
        #region Fields
        private readonly IUnitOfWork _unitOfWork;
        private readonly IImageHelper _imageHelper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ClaimsPrincipal _user;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IToastMsg _toast;
        #endregion

        #region Properties
        public IRepository<AppUser> UserRepo => _unitOfWork.GetRepository<AppUser>();
        #endregion

        #region Ctor
        public UserService(IUnitOfWork unitOfWork, IImageHelper imageHelper, IHttpContextAccessor httpContextAccessor, IMapper mapper, UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager, RoleManager<AppRole> roleManager, IToastMsg toast)
        {
            _unitOfWork = unitOfWork;
            _imageHelper = imageHelper;
            _httpContextAccessor = httpContextAccessor;
            _user = _httpContextAccessor.HttpContext.User;
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _toast = toast;
        }
        #endregion


        public async Task<List<AppRole>> GetAllRolesAsync()
        {
            return await _roleManager.Roles
                .ToListAsync();
        }

        public async Task<List<UserDto>> GetAllUsersAsync(Expression<Func<AppUser, bool>> predicate = null)
        {
            var users = await _userManager.Users
                .Where(predicate)
                .ToListAsync();
            return _mapper.Map<List<UserDto>>(users);
        }

        public async Task<List<UserDto>> GetAllUsersWithRoleAsync(Expression<Func<AppUser, bool>> predicate = null)
        {
            var users = predicate != null
                ? await _userManager.Users
                    .Where(predicate)
                    .ToListAsync()
                : await _userManager.Users
                    .ToListAsync();
            var map = _mapper.Map<List<UserDto>>(users);

            foreach (var user in map)
            {
                var findUser = await _userManager
                    .FindByIdAsync(user.Id.ToString());
                var role = await _userManager
                    .GetRolesAsync(findUser);

                if (role.First().ToUpper() != RoleConst.SuperAdmin.ToUpper())
                    user.Role = role.First();
                else
                    user.Role = null;
            }

            return map.Where(_ => _.Role != null).ToList();
        }

        public async Task<AppUser> GetAppUserByIdAsync(Guid userId)
        {
            return await _userManager
                .FindByIdAsync(userId.ToString());
        }

        public async Task<AppUser> GetAppUserByEmailAsync(string email)
        {
            return await _userManager
                .FindByEmailAsync(email);
        }

        public async Task<AppUser> GetAppUserByIdIncludeImageAsync(Guid userId)
        {
            AppUser user;
            try
            {
                user = await UserRepo
                    .GetAsync(_ => _.Id == userId, i => i.Image);
            }
            catch
            {
                return null;
            }

            return user;
        }

        public async Task<string> GetUserRoleAsync(AppUser user)
        {
            return (await _userManager
                .GetRolesAsync(user))
                .First();
        }

        public async Task<UserProfileDto> GetUserProfileAsync()
        {
            var userId = _user.GetLoggedInUserId();
            var getUserWithImage = await UserRepo
                .GetAsync(_ => _.Id == userId, _ => _.Image);
            var map = _mapper.Map<UserProfileDto>(getUserWithImage);
            map.Image.FileName = getUserWithImage.Image.FileName;

            return map;
        }

        public async Task<bool> UserProfileUpdateAsync(UserProfileDto userProfileDto)
        {
            var userId = _user.GetLoggedInUserId();
            var user = await GetAppUserByIdAsync(userId);
            var isVerified = await _userManager
                .CheckPasswordAsync(user, userProfileDto.CurrentPassword);

            if (isVerified && userProfileDto.NewPassword != null)
            {

                var result = await _userManager.ChangePasswordAsync(user, userProfileDto.CurrentPassword, userProfileDto.NewPassword);
                if (result.Succeeded)
                {
                    await _userManager
                        .UpdateSecurityStampAsync(user);
                    await _signInManager
                        .SignOutAsync();
                    await _signInManager
                        .PasswordSignInAsync(user, userProfileDto.NewPassword, true, false);

                    _mapper.Map(userProfileDto, user);
                    if (userProfileDto.Photo != null)
                    {
                        user.ImageId = await UploadImageForUserAsync(userProfileDto);
                    }

                    var isSuccess = await _userManager.UpdateAsync(user);
                    if (isSuccess.Succeeded)
                    {
                        _toast.Success(Messages.GlobalMessage.Success);
                        return true;
                    }
                }
                _toast.Error();
                return false;
            }
            else if (isVerified)
            {
                await _userManager.UpdateSecurityStampAsync(user);
                Guid imageId = user.ImageId;
                _mapper.Map(userProfileDto, user);
                user.ImageId = imageId;
                if (userProfileDto.Photo != null)
                {
                    user.ImageId = await UploadImageForUserAsync(userProfileDto);
                }

                var isSuccess = await _userManager.UpdateAsync(user);
                if (isSuccess.Succeeded)
                {
                    _toast.Success(Messages.GlobalMessage.Success);
                    return true;
                }

                _toast.Error();
                return true;
            }

            return false;
        }

        #region Create
        public async Task<IdentityResult> CreateUserAsync(UserAddDto userAddDto)
        {
            var map = _mapper.Map<AppUser>(userAddDto);
            map.UserName = userAddDto.Email;
            map.IsDeleted = false;

            var imageUpload = await _imageHelper.Upload(userAddDto.Email, userAddDto.Photo, ImageType.User);
            Image image = new(imageUpload.FullName, userAddDto.Photo.ContentType, _user.GetLoggedInEmail());
            await _unitOfWork.GetRepository<Image>().AddAsync(image);
            await _unitOfWork.SaveAsync();

            map.ImageId = image.Id;
            var createResult = await _userManager
                .CreateAsync(map, string.IsNullOrEmpty(userAddDto.Password) ? "" : userAddDto.Password);

            if (createResult.Succeeded)
            {
                var role = await _roleManager.FindByIdAsync(userAddDto.RoleId.ToString());
                string findRole = role != null
                    ? role.Name
                    : RoleConst.User;

                if (findRole.ToUpper().Equals(RoleConst.SuperAdmin.ToUpper()))
                    findRole = RoleConst.User;

                await _userManager.AddToRoleAsync(map, findRole);
                _toast.Success(Messages.User.Add(map.Email));
            }

            return createResult;
        }
        #endregion

        #region Update
        public async Task<(IdentityResult identityResult, string userName)> UpdateUserAsync(UserUpdateDto userUpdateDto)
        {
            AppUser user = await UserRepo
                .GetAsync(_ => _.Id == userUpdateDto.Id, i => i.Image);
            var role = await GetUserRoleAsync(user);

            if (!await _userManager.IsInRoleAsync(user, RoleConst.SuperAdmin.ToUpper()))
                await _userManager.RemoveFromRoleAsync(user, role);

            Image userImage = user.Image;

            if (userUpdateDto.Photo != null)
            {
                var imageUpload = await _imageHelper
                    .Upload(userUpdateDto.Email, userUpdateDto.Photo, ImageType.User);
                Image image = new(imageUpload.FullName, userUpdateDto.Photo.ContentType, userUpdateDto.Email);
                await _unitOfWork
                    .GetRepository<Image>()
                    .AddAsync(image);
                await _unitOfWork.SaveAsync();

                _imageHelper.Delete(userImage.FileName);

                userImage = image;
            }


            user.ImageId = userImage.Id;
            user.FirstName = userUpdateDto.FirstName;
            user.LastName = userUpdateDto.LastName;
            user.Email = userUpdateDto.Email;
            user.PhoneNumber = userUpdateDto.PhoneNumber;

            user.UserName = userUpdateDto.Email;
            user.NormalizedEmail = user.Email.ToUpper();
            user.NormalizedUserName = user.UserName.ToUpper();
            user.SecurityStamp = Guid.NewGuid().ToString();

            var findRole = await _roleManager.FindByIdAsync(userUpdateDto.RoleId.ToString());

            if (findRole.NormalizedName != RoleConst.SuperAdmin.ToUpper())
                await _userManager.AddToRoleAsync(user, findRole.Name);

            var updateResult = await _userManager.UpdateAsync(user);
            if (updateResult.Succeeded)
                _toast.Success(Messages.User.Update(userUpdateDto.Email));
            else
                _toast.Error();

            return (updateResult, userUpdateDto.Email);
        }
        #endregion

        #region Delete & UndoDelete
        public async Task<string> UndoDeleteAsync(Guid userId)
        {
            AppUser user;

            try
            {
                user = await UserRepo.GetAsync(_ => _.Id == userId);
            }
            catch
            {
                return null;
            }

            if (user != null)
            {
                user.IsDeleted = false;
                var isSuccess = await _userManager.UpdateAsync(user);
                if (isSuccess.Succeeded)
                {
                    _toast.Success(Messages.User.UndoDelete(user.FirstName));
                    return user.Email;

                }
                _toast.Error();
            }

            return null;
        }

        public async Task<(IdentityResult identityResult, string? email)> SafeDeleteUserAsync(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user != null)
            {
                user.IsDeleted = true;
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                    _toast.Success(Messages.User.Delete(user.Email));

                return result.Succeeded
                    ? (result, user.Email)
                    : (result, null);
            }

            return (null, null);
        }
        #endregion

        #region Private Methods
        private async Task<Guid> UploadImageForUserAsync(UserProfileDto userProfileDto)
        {
            var userEmail = _user.GetLoggedInEmail();

            var imageUpload = await _imageHelper
                .Upload($"{userProfileDto.FirstName}{userProfileDto.LastName}", userProfileDto.Photo, ImageType.User);
            Image image = new(imageUpload.FullName, userProfileDto.Photo.ContentType, userEmail);
            await _unitOfWork
                .GetRepository<Image>()
                .AddAsync(image);

            return image.Id;
        }
        #endregion 
    }
}
