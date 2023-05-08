using BlogWebSite.Entity.Models.DTOs.Articles;
using BlogWebSite.Entity.Models.DTOs.Comments;

namespace BlogWebSite.Entity.Models.DTOs.ArticleComment
{
    public class ArticleCommentDto
    {
        public Guid CommentId { get; set; }
        public CommentDto Comment { get; set; }
        public Guid ArticleId { get; set; }
        public ArticleDto Article { get; set; }
        public Guid UserId { get; set; }
        public virtual string CreatedBy { get; set; } = "Undefined";
        public virtual DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
