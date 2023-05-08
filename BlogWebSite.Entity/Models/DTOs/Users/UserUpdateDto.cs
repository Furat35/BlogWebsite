using BlogWebSite.Entity.Entities.Concrete;
using BlogWebSite.Entity.Models.DTOs.Roles;
using Microsoft.AspNetCore.Http;

namespace BlogWebSite.Entity.Models.DTOs.Users
{
    public class UserUpdateDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public Guid RoleId { get; set; }
        public List<RoleDto> Roles { get; set; }
        public Image Image { get; set; }
        public IFormFile Photo { get; set; }
    }
}
