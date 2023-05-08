using BlogWebSite.Entity.Entities.Concrete;
using System.Linq.Expressions;

namespace BlogWebSite.Service.Services.Abstract
{
    public interface IDashboardService
    {
        Task<List<int>> GetYearlyArticleCountsAsync();
        Task<int> GetTotalArticleCountAsync(Expression<Func<Article, bool>> predicate = null);
        Task<int> GetTotalCategoryCountAsync(Expression<Func<Category, bool>> predicate = null);
        Task<int> GetAllUsersAsync(Expression<Func<AppUser, bool>> predicate = null);
    }
}
