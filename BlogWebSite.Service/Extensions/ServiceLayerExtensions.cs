using BlogWebSite.Service.FluentValidations;
using BlogWebSite.Service.Helpers.Images;
using BlogWebSite.Service.Helpers.ToastMessage;
using BlogWebSite.Service.Services.Abstract;
using BlogWebSite.Service.Services.Concrete;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using System.Reflection;

namespace BlogWebSite.Service.Extensions
{
    public static class ServiceLayerExtensions
    {
        public static void AddServiceLayerExtension(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();

            services.AddScoped<IArticleService, ArticleService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IImageHelper, ImageHelper>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IDashboardService, DashboardService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<IToastMsg, ToastMsg>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            #region Fluent Validation
            services.AddControllersWithViews()
                .AddFluentValidation(opt =>
            {
                opt.RegisterValidatorsFromAssemblyContaining<ArticleValidator>();
                opt.DisableDataAnnotationsValidation = true;
                opt.ValidatorOptions.LanguageManager.Culture = new CultureInfo("tr");
            });
            #endregion

            #region AutoMapper
            services.AddAutoMapper(assembly);
            #endregion
        }


    }
}
