using BlogWebSite.Entity.Models.DTOs.Articles;

namespace BlogWebSite.Service.Services.Abstract
{
    public interface IArticleService
    {
        Task<List<ArticleDto>> GetAllArticlesWithCategoryNonDeletedAsync();
        Task<List<ArticleDto>> GetAllDeletedArticlesWithCategoryAsync();
        Task CreateArticleAsync(ArticleAddDto articleAddDto);
        Task<ArticleDto> GetArticleWithCategoryNonDeletedAsync(Guid articleId);
        Task UpdateArticleAsync(ArticleUpdateDto articleUpdateDto);
        Task SafeDeleteArticleAsync(Guid articleId);
        Task UndoDeleteAsync(Guid articleId);
        Task<ArticleListDto> GetAllByPaggingAsync(Guid? categoryId, int currentPage = 1, int pageSize = 3, bool isAscending = false);
        Task<ArticleListDto> SearchAsync(string keyword, int currentPage = 1, int pageSize = 3, bool isAscending = false);
        Task<List<ArticleDto>> GetLastNPostsAsync(int count);
    }
}
