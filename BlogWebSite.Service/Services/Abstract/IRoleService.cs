using BlogWebSite.Entity.Entities.Concrete;
using BlogWebSite.Entity.Models.DTOs.Roles;
using System.Linq.Expressions;

namespace BlogWebSite.Service.Services.Abstract
{
    public interface IRoleService
    {
        Task<List<RoleDto>> GetAllRolesAsync(Expression<Func<AppRole, bool>> predicate = null);
        Task CreateRoleAsync(RoleAddDto roleDto);
        Task DeleteRoleAsync(Guid roleId);
        Task UpdateRoleAsync(RoleUpdateDto roleUpdateDto);
        Task<AppRole> GetRoleByGuidAsync(Guid roleId);
        Task<string> GetRoleGuidAsync(string name);
        Task SafeDeleteRoleAsync(Guid roleId);
    }
}
