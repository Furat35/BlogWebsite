using BlogWebSite.Core.Entities.Concrete;

namespace BlogWebSite.Entity.Entities.Concrete
{
    public class Category : EntityBase
    {
        public Category()
        {

        }
        public Category(string name, string createdBy)
        {
            Name = name;
            CreatedBy = createdBy;
        }
        public string Name { get; set; }
        public ICollection<Article>? Articles { get; set; }


    }

}
