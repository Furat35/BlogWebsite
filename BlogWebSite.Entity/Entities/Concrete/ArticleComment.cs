using BlogWebSite.Core.Entities.Concrete;

namespace BlogWebSite.Entity.Entities.Concrete
{
    public class ArticleComment : EntityBase
    {
        public Guid? CommentId { get; set; }
        public Comment? Comment { get; set; }
        public Guid? ArticleId { get; set; }
        public Article? Article { get; set; }
    }
}
