using AutoMapper;
using BlogWebSite.Core.Const;
using BlogWebSite.Core.ResultMessages;
using BlogWebSite.Data.Repositories.Abstract;
using BlogWebSite.Data.UnitOfWorks;
using BlogWebSite.Entity.Entities.Concrete;
using BlogWebSite.Entity.Models.DTOs.Roles;
using BlogWebSite.Service.Helpers.ToastMessage;
using BlogWebSite.Service.Services.Abstract;
using System.Data;
using System.Linq.Expressions;

namespace BlogWebSite.Service.Services.Concrete
{
    public class RoleService : IRoleService
    {
        #region Fields
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IToastMsg _toast;
        #endregion

        #region Properties
        public IRepository<AppRole> RoleRepo => _unitOfWork
            .GetRepository<AppRole>();
        #endregion

        #region Ctor
        public RoleService(IUnitOfWork unitOfWork, IMapper mapper, IToastMsg toast)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _toast = toast;
        }
        #endregion

        #region Create
        public async Task CreateRoleAsync(RoleAddDto roleDto)
        {
            var role = await RoleRepo
                .GetAsync(_ => _.Name.ToUpper() == roleDto.Name.ToUpper());
            if (role is null)
            {
                var map = _mapper.Map<AppRole>(roleDto);
                map.NormalizedName = roleDto.Name.ToUpper();
                map.ConcurrencyStamp = Guid.NewGuid().ToString();
                await RoleRepo.AddAsync(map);
            }
            else if (role.IsDeleted)
            {
                role.IsDeleted = false;
                await RoleRepo.UpdateAsync(role);
            }

            await ToastMessage(Messages.Role.Add(roleDto.Name));
        }
        #endregion

        #region Delete
        public async Task SafeDeleteRoleAsync(Guid roleId)
        {
            var role = await RoleRepo
                .GetByGuidAsync(roleId);
            if (!role.IsDeleted &&
                !role.Name.ToUpper().Equals(RoleConst.SuperAdmin.ToUpper()))
            {
                role.IsDeleted = true;
                await RoleRepo.UpdateAsync(role);
                await ToastMessage(role.Name);
            }
        }

        public async Task DeleteRoleAsync(Guid roleId)
        {
            var role = await RoleRepo
                .GetByGuidAsync(roleId);
            if (!role.IsDeleted &&
                !role.Name.ToUpper().Equals(RoleConst.SuperAdmin.ToUpper()))
            {
                await RoleRepo.DeleteAsync(role);
                await ToastMessage(Messages.Role.Delete(role.Name));
            }
        }
        #endregion

        #region Update
        public async Task UpdateRoleAsync(RoleUpdateDto roleUpdateDto)
        {
            var role = await RoleRepo
                .GetByGuidAsync(roleUpdateDto.Id);
            if (role is not null &&
                !role.Name.ToUpper().Equals(RoleConst.SuperAdmin.ToUpper()))
            {
                role.Name = roleUpdateDto.Name;
                role.NormalizedName = roleUpdateDto.Name.ToUpper();
                role.ConcurrencyStamp = Guid.NewGuid().ToString();
                await RoleRepo.UpdateAsync(role);
                await ToastMessage(Messages.Role.Update(role.Name));
            }
        }
        #endregion

        public async Task<AppRole> GetRoleByGuidAsync(Guid roleId)
        {
            var role = await RoleRepo
                .GetByGuidAsync(roleId);
            return role;
        }

        public async Task<List<RoleDto>> GetAllRolesAsync(Expression<Func<AppRole, bool>> predicate = null)
        {
            var roles = (await RoleRepo
                .GetAllAsync(predicate))
                .Where(_ => _.NormalizedName != RoleConst.SuperAdmin.ToUpper());
            return _mapper.Map<List<RoleDto>>(roles);
        }

        public async Task<string> GetRoleGuidAsync(string name)
        {
            var role = await RoleRepo
                .GetAsync(_ => _.NormalizedName == name.ToUpper());
            return role?.Id.ToString();
        }

        #region Private Methods
        private async Task ToastMessage(string message)
        {
            if (await SaveAsync())
                _toast.Success(message);
            else
                _toast.Error();
        }

        private async Task<bool> SaveAsync()
        {
            int effectedRows = await _unitOfWork.SaveAsync();
            return effectedRows > 0
                ? true
                : false;
        }
        #endregion
    }
}
