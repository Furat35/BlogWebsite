using AutoMapper;
using BlogWebSite.Core.ResultMessages;
using BlogWebSite.Data.Repositories.Abstract;
using BlogWebSite.Data.UnitOfWorks;
using BlogWebSite.Entity.Entities.Concrete;
using BlogWebSite.Entity.Enums;
using BlogWebSite.Entity.Models.DTOs.Articles;
using BlogWebSite.Service.Extensions;
using BlogWebSite.Service.Helpers.Images;
using BlogWebSite.Service.Helpers.ToastMessage;
using BlogWebSite.Service.Services.Abstract;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace BlogWebSite.Service.Services.Concrete
{
    public class ArticleService : IArticleService
    {
        #region Fields
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ClaimsPrincipal _user;
        private readonly IImageHelper _imageHelper;
        private readonly IToastMsg _toast;
        #endregion

        #region Properties
        public IRepository<Article> ArticleRepo => _unitOfWork.GetRepository<Article>();
        #endregion

        #region Ctor
        public ArticleService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor, IImageHelper imageHelper,
            IToastMsg toast)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _user = _httpContextAccessor.HttpContext.User;
            _imageHelper = imageHelper;
            _toast = toast;
        }
        #endregion

        public async Task<List<ArticleDto>> GetLastNPostsAsync(int count)
        {
            var articles = (await ArticleRepo
                .GetAllAsync())
                .OrderByDescending(_ => _.CreatedDate)
                .Take(count);
            return _mapper.Map<List<ArticleDto>>(articles);
        }

        public async Task<ArticleListDto> GetAllByPaggingAsync(Guid? categoryId, int currentPage = 1, int pageSize = 3, bool isAscending = false)
        {
            pageSize = pageSize > 20
                ? 20
                : pageSize;

            var articles = categoryId == null
                ? await ArticleRepo
                .GetAllAsync(_ => !_.isDeleted, c => c.Category, i => i.Image, u => u.User)
                : await ArticleRepo.
                GetAllAsync(_ => _.CategoryId == categoryId && !_.isDeleted, c => c.Category, i => i.Image, u => u.User);

            var sortedArticles = isAscending
                ? articles
                .OrderBy(_ => _.CreatedDate)
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .ToList()
                : articles
                .OrderByDescending(_ => _.CreatedDate)
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new ArticleListDto
            {
                Articles = sortedArticles,
                CategoryId = categoryId,
                CurrentPage = currentPage,
                PageSize = pageSize,
                TotalCount = articles.Count,
                IsAscending = isAscending
            };
        }

        public async Task<List<ArticleDto>> GetAllDeletedArticlesWithCategoryAsync()
        {
            var articles = await ArticleRepo
                .GetAllAsync(predicate: _ => _.isDeleted, include => include.Category, include => include.Image);
            return _mapper.Map<List<ArticleDto>>(articles);
        }

        public async Task<ArticleListDto> SearchAsync(string keyword, int currentPage = 1, int pageSize = 3, bool isAscending = false)
        {
            pageSize = pageSize > 20
                ? 20
                : pageSize;

            var articles = await ArticleRepo
                .GetAllAsync(
                _ => !_.isDeleted && (_.Title.Contains(keyword) || _.Content.Contains(keyword) || _.Category.Name.Contains(keyword)), c => c.Category, i => i.Image, u => u.User);

            var sortedArticles = isAscending
                ? articles
                    .OrderBy(_ => _.CreatedDate)
                    .Skip((currentPage - 1) * pageSize)
                    .Take(pageSize)
                    .ToList()
                : articles
                    .OrderByDescending(_ => _.CreatedDate)
                    .Skip((currentPage - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

            return new ArticleListDto
            {
                Articles = sortedArticles,
                CurrentPage = currentPage,
                PageSize = pageSize,
                TotalCount = articles.Count,
                IsAscending = isAscending
            };
        }

        public async Task<List<ArticleDto>> GetAllArticlesWithCategoryNonDeletedAsync()
        {
            var articles = await ArticleRepo
                .GetAllAsync(_ => !_.isDeleted, include => include.Category);
            return _mapper.Map<List<ArticleDto>>(articles);
        }

        public async Task<ArticleDto> GetArticleWithCategoryNonDeletedAsync(Guid articleId)
        {
            Article article = await ArticleExistAsync(articleId, false);
            return article != null
                ? _mapper.Map<ArticleDto>(article)
                : null;
        }

        #region Create
        public async Task CreateArticleAsync(ArticleAddDto articleAddDto)
        {
            var userId = _user.GetLoggedInUserId();
            var userEmail = _user.GetLoggedInEmail();

            var imageUpload = await _imageHelper.Upload(articleAddDto.Title, articleAddDto.Photo, ImageType.Post);
            Image image = new(imageUpload.FullName, articleAddDto.Photo.ContentType, userEmail);
            await _unitOfWork
                .GetRepository<Image>()
                .AddAsync(image);

            var article = new Article(articleAddDto.Title, articleAddDto.Content, userId, userEmail, articleAddDto.CategoryId, image.Id);

            await ArticleRepo.AddAsync(article);
            await ToastMessage(Messages.Article.Add(articleAddDto.Title));
        }
        #endregion

        #region Update
        public async Task UpdateArticleAsync(ArticleUpdateDto articleUpdateDto)
        {
            Article article = await ArticleExistAsync(articleUpdateDto.Id, false);

            if (article != null)
            {
                if (articleUpdateDto.Photo != null)
                {
                    if (article.Image != null)
                    {
                        article.Image.isDeleted = true;
                        _imageHelper.Delete(article.Image.FileName);
                    }
                    var imageUploade = await _imageHelper
                        .Upload(articleUpdateDto.Title, articleUpdateDto.Photo, ImageType.Post);
                    Image image = new(imageUploade.FullName, articleUpdateDto.Photo.ContentType, _user.GetLoggedInEmail());
                    await _unitOfWork
                        .GetRepository<Image>()
                        .AddAsync(image);

                    article.ImageId = image.Id;
                }

                string articleTitle = article.Title;

                article.Title = articleUpdateDto.Title;
                article.CategoryId = articleUpdateDto.CategoryId;
                article.Content = articleUpdateDto.Content;

                article.ModifiedDate = DateTime.Now;
                article.ModifiedBy = _user.GetLoggedInEmail();

                await ArticleRepo.UpdateAsync(article);
                await ToastMessage(Messages.Article.Update(articleTitle));

            }
        }
        #endregion

        #region Delete & UndoDelete
        public async Task SafeDeleteArticleAsync(Guid articleId)
        {
            Article article = await ArticleExistAsync(articleId, false);

            if (article != null && !article.isDeleted)
            {
                article.isDeleted = true;
                article.DeletedDate = DateTime.Now;
                article.DeletedBy = _user.GetLoggedInEmail();
                await ArticleRepo.UpdateAsync(article);
                await ToastMessage(Messages.Article.Add(article.Title));
            }

        }

        public async Task UndoDeleteAsync(Guid articleId)
        {
            Article? article = await ArticleExistAsync(articleId, true);

            if (article != null)
            {
                article.isDeleted = false;
                article.DeletedDate = null;
                article.DeletedBy = null;
                await ArticleRepo.UpdateAsync(article);
                await ToastMessage(Messages.Article.UndoDelete(article.Title));
            }
        }
        #endregion

        #region Private Methods
        private async Task ToastMessage(string message)
        {
            if (await SaveAsync())
                _toast.Success(message);
            else
                _toast.Error();
        }
        private async Task<bool> SaveAsync()
        {
            int effectedRows = await _unitOfWork.SaveAsync();
            return effectedRows > 0
                ? true
                : false;
        }
        private async Task<Article?> ArticleExistAsync(Guid articleId, bool isDeleted)
        {
            try
            {
                Article article = await ArticleRepo
                    .GetAsync(predicate: _ => _.isDeleted == isDeleted && _.Id == articleId, include => include.Category, include => include.Image);
                return article;
            }
            catch
            {
                return null;
            }
        }
        #endregion

    }
}
