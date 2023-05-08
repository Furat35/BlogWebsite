using AutoMapper;
using BlogWebSite.Data.Repositories.Abstract;
using BlogWebSite.Data.UnitOfWorks;
using BlogWebSite.Entity.Entities.Concrete;
using BlogWebSite.Entity.Models.DTOs.ArticleComment;
using BlogWebSite.Entity.Models.DTOs.Comments;
using BlogWebSite.Entity.Models.DTOs.Users;
using BlogWebSite.Service.Extensions;
using BlogWebSite.Service.Helpers.ToastMessage;
using BlogWebSite.Service.Services.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace BlogWebSite.Service.Services.Concrete
{
    public class MessageService : IMessageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;
        private readonly IToastMsg _toast;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IMapper _mapper;

        private IRepository<Comment> CommentRepo =>
            _unitOfWork.GetRepository<Comment>();
        private IRepository<ArticleComment> ArticleRepo =>
            _unitOfWork.GetRepository<ArticleComment>();

        public MessageService(IUnitOfWork unitOfWork, UserManager<AppUser> userManager, IToastMsg toast, IHttpContextAccessor httpContext,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _toast = toast;
            _httpContext = httpContext;
            _mapper = mapper;
        }

        public async Task<CommentDto> CreateMessageAsync(string message, Guid articleId)
        {
            Guid userId = _httpContext.HttpContext.User
                .GetLoggedInUserId();
            Comment comment = new Comment();
            comment.UserId = userId;
            comment.Content = message;
            comment.User = await _userManager.GetUserAsync(_httpContext.HttpContext.User);

            await CommentRepo
                .AddAsync(comment);
            ArticleComment articleComment = new ArticleComment
            {
                ArticleId = articleId,
                Comment = comment,
                CreatedBy = userId.ToString()
            };
            await ArticleRepo.AddAsync(articleComment);
            await ToastMessage("Basarilii");
            return _mapper.Map<CommentDto>(comment);
        }

        public async Task<List<ArticleCommentDto>> GetMessagesByArticleAsync(Guid articleId)
        {
            var articleComments = _mapper.Map<List<ArticleCommentDto>>(
                await ArticleRepo
                .GetAllAsync(_ => _.ArticleId == articleId, _ => _.Comment));
            foreach (var articleComment in articleComments)
            {
                articleComment.Comment.User = _mapper.Map<UserDto>(_userManager.Users
                    .Where(_ => _.Id == articleComment.Comment.UserId)
                    .First());
            }

            return articleComments.OrderByDescending(_ => _.CreatedDate).ToList();
        }

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
        #endregion
    }
}
