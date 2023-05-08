using BlogWebSite.Entity.Entities.Concrete;
using BlogWebSite.Entity.Models.DTOs.Categories;
using Microsoft.AspNetCore.Http;

namespace BlogWebSite.Entity.Models.DTOs.Articles
{
    public class ArticleUpdateDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public Guid CategoryId { get; set; }

        public Image Image { get; set; }
        public IFormFile? Photo { get; set; }
        public IList<CategoryDto>? Categories { get; set; }
    }
}
