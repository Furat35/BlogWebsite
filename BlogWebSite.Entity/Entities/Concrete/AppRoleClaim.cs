using BlogWebSite.Core.Entities.Abstract;
using Microsoft.AspNetCore.Identity;

namespace BlogWebSite.Entity.Entities.Concrete
{
    public class AppRoleClaim : IdentityRoleClaim<Guid>, IEntityBase
    {
    }
}
