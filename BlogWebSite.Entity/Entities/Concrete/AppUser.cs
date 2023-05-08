using BlogWebSite.Core.Entities.Abstract;
using Microsoft.AspNetCore.Identity;

namespace BlogWebSite.Entity.Entities.Concrete
{
    public class AppUser : IdentityUser<Guid>, IEntityBase
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Guid ImageId { get; set; }
        public Image Image { get; set; }
        public ICollection<Article> Articles { get; set; }
        public bool IsDeleted { get; set; }
    }
}
