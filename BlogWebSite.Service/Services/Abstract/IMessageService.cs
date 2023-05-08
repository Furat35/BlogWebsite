using BlogWebSite.Entity.Models.DTOs.ArticleComment;
using BlogWebSite.Entity.Models.DTOs.Comments;

namespace BlogWebSite.Service.Services.Abstract
{
    public interface IMessageService
    {
        Task<CommentDto> CreateMessageAsync(string message, Guid articleId);
        Task<List<ArticleCommentDto>> GetMessagesByArticleAsync(Guid articleId);
        //Task DeleteMessage();
        //Task UpdateMessage();
    }
}
