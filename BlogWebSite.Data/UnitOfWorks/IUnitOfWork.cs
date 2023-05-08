using BlogWebSite.Core.Entities.Abstract;
using BlogWebSite.Data.Repositories.Abstract;

namespace BlogWebSite.Data.UnitOfWorks
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        IRepository<T> GetRepository<T>() where T : class, IEntityBase, new();
        Task<int> SaveAsync();
        int Save();
    }
}
