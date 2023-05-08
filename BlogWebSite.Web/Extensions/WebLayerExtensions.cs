using BlogWebSite.Data.Context;
using BlogWebSite.Entity.Entities.Concrete;
using BlogWebSite.Service.Describers;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NToastNotify;

namespace BlogWebSite.Web.Extensions
{
    public static class WebLayerExtensions
    {
        public static void AddWebLayerExtension(this IServiceCollection services)
        {


            services.AddControllersWithViews()
                  .AddNewtonsoftJson(options =>
                  {
                      // Use the default property (Pascal) casing
                      options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                      options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                  })
                .AddNToastNotifyToastr(new ToastrOptions
                {
                    PositionClass = ToastPositions.TopRight,
                    TimeOut = 3000
                })
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                }); ;

            #region Identity Framework
            services.AddIdentity<AppUser, AppRole>(opt =>
            {
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireLowercase = false;
            })
            .AddRoleManager<RoleManager<AppRole>>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders()
            .AddErrorDescriber<CustomIdentityErrorDescriber>();
            #endregion

            #region Session
            services.AddSession();
            #endregion

            #region Cookie Config
            services.ConfigureApplicationCookie(config =>
            {
                config.LoginPath = new PathString("/Auth/Login");
                config.LogoutPath = new PathString("/Auth/Logout");
                config.Cookie = new CookieBuilder
                {
                    Name = "YoutubeBlog",
                    HttpOnly = true,
                    SameSite = SameSiteMode.Strict,
                    SecurePolicy = CookieSecurePolicy.SameAsRequest,
                };
                config.SlidingExpiration = true;
                config.ExpireTimeSpan = TimeSpan.FromDays(7);
                config.AccessDeniedPath = new PathString("/Admin/Auth/AccessDenied");
            });
            #endregion
        }
        public static void UseWebLayerExtension(this WebApplication app)
        {
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseStatusCodePagesWithRedirects("/Home/InvalidPage?statusCode={0}");
            app.UseNToastNotify();

            app.UseRouting();

            app.UseSession();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapAreaControllerRoute(
                name: "Admin",
                areaName: "Admin",
                pattern: "Admin/{controller=Home}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");


        }
    }
}
