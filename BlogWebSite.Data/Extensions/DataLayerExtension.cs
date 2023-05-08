using BlogWebSite.Data.Context;
using BlogWebSite.Data.Repositories.Abstract;
using BlogWebSite.Data.Repositories.Concrete;
using BlogWebSite.Data.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BlogWebSite.Data.Extensions
{
    public static class DataLayerExtension
    {
        public static void AddDataLayerExtension(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            #region DbContext
            services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(config.GetConnectionString("DefaultConnection")));
            #endregion
        }
    }
}
