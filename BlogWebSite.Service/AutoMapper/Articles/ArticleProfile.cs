using AutoMapper;
using BlogWebSite.Entity.Entities.Concrete;
using BlogWebSite.Entity.Models.DTOs.Articles;

namespace BlogWebSite.Service.AutoMapper.Articles
{
    public class ArticleProfile : Profile
    {
        public ArticleProfile()
        {
            CreateMap<Article, ArticleDto>().ReverseMap();
            CreateMap<Article, ArticleAddDto>().ReverseMap();
            CreateMap<Article, ArticleUpdateDto>().ReverseMap();
            CreateMap<ArticleDto, ArticleUpdateDto>().ReverseMap();
        }
    }
}
