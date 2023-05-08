using BlogWebSite.Entity.Enums;
using BlogWebSite.Entity.Models.DTOs.Images;
using Microsoft.AspNetCore.Http;

namespace BlogWebSite.Service.Helpers.Images
{
    public interface IImageHelper
    {
        Task<ImageUploadDto> Upload(string name, IFormFile imageFile, ImageType imageType, string folderName = null);
        void Delete(string imageName);
    }
}
