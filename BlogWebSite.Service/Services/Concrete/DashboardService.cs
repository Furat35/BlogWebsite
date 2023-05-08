using BlogWebSite.Data.Repositories.Abstract;
using BlogWebSite.Data.UnitOfWorks;
using BlogWebSite.Entity.Entities.Concrete;
using BlogWebSite.Service.Services.Abstract;
using System.Linq.Expressions;

namespace BlogWebSite.Service.Services.Concrete
{
    public class DashboardService : IDashboardService
    {
        #region Fields
        private readonly IUnitOfWork _unitOfWork;
        #endregion

        #region Properties
        public IRepository<Article> ArticleRepo => _unitOfWork
            .GetRepository<Article>();
        #endregion

        #region Ctor
        public DashboardService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        #endregion

        public async Task<List<int>> GetYearlyArticleCountsAsync()
        {
            var articles = await ArticleRepo
                .GetAllAsync(_ => !_.isDeleted);

            var startDate = DateTime.Now.Date;
            startDate = new DateTime(startDate.Year, 1, 1);

            List<int> datas = new();
            for (int i = 1; i <= 12; i++)
            {
                var startedDate = new DateTime(startDate.Year, i, 1);
                var endedDate = startedDate.AddMonths(1);
                var data = articles
                    .Where(_ => _.CreatedDate >= startedDate && _.CreatedDate < endedDate)
                    .Count();
                datas.Add(data);
            }

            return datas;
        }

        public async Task<int> GetTotalArticleCountAsync(Expression<Func<Article, bool>> predicate = null)
        {
            int articleCount = await ArticleRepo
                .CountAsync(predicate);
            return articleCount;
        }

        public async Task<int> GetTotalCategoryCountAsync(Expression<Func<Category, bool>> predicate = null)
        {
            int categoryCount = await _unitOfWork
                .GetRepository<Category>()
                .CountAsync(predicate);
            return categoryCount;
        }

        public async Task<int> GetAllUsersAsync(Expression<Func<AppUser, bool>> predicate = null)
        {
            var users = await _unitOfWork
                .GetRepository<AppUser>()
                .GetAllAsync(predicate);
            return users.Count;
        }
    }
}
