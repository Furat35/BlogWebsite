using AutoMapper;
using BlogWebSite.Entity.Entities.Concrete;
using BlogWebSite.Entity.Models.DTOs.ArticleComment;

namespace BlogWebSite.Service.AutoMapper.ArticleComments
{
    public class ArticleCommentProfile : Profile
    {
        public ArticleCommentProfile()
        {
            CreateMap<ArticleComment, ArticleCommentDto>();
        }
    }
}
