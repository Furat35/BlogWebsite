using BlogWebSite.Entity.Models.DTOs.ArticleComment;
using BlogWebSite.Entity.Models.DTOs.Users;

namespace BlogWebSite.Entity.Models.DTOs.Comments
{
    public class CommentDto
    {
        public string Content { get; set; }
        public Guid? UserId { get; set; }
        public UserDto? User { get; set; }
        public ICollection<ArticleCommentDto> ArticleComments { get; set; }
        public virtual DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
