using BlogWebSite.Entity.Models.DTOs.ArticleComment;
using BlogWebSite.Entity.Models.DTOs.Users;

namespace BlogWebSite.Entity.Models.DTOs.Comments
{
    public class CommentAddDto
    {
        public string Content { get; set; }
        public UserDto? User { get; set; }
        public ICollection<ArticleCommentDto> ArticleComments { get; set; }
    }
}
