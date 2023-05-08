using BlogWebSite.Core.Entities.Concrete;

namespace BlogWebSite.Entity.Entities.Concrete
{
    public class Comment : EntityBase
    {
        public Comment()
        {

        }
        public Comment(string content)
        {
            Content = content;
        }
        public string Content { get; set; }
        public Guid? UserId { get; set; }
        public AppUser? User { get; set; }
        public ICollection<ArticleComment> ArticleComments { get; set; }
    }
}
