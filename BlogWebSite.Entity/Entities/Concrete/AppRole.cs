using BlogWebSite.Core.Entities.Abstract;
using Microsoft.AspNetCore.Identity;

namespace BlogWebSite.Entity.Entities.Concrete
{
    public class AppRole : IdentityRole<Guid>, IEntityBase
    {
        public bool IsDeleted { get; set; } = false;
    }
}
