﻿using BlogWebSite.Core.Entities.Abstract;
using Microsoft.AspNetCore.Identity;

namespace BlogWebSite.Entity.Entities.Concrete
{
    public class AppUserRole : IdentityUserRole<Guid>, IEntityBase
    {
    }
}
