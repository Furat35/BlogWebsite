using BlogWebSite.Entity.Enums;
using BlogWebSite.Entity.Models.DTOs.Images;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace BlogWebSite.Service.Helpers.Images
{
    public class ImageHelper : IImageHelper
    {
        #region Fields
        private readonly IWebHostEnvironment _env;
        private readonly string wwwroot;
        private const string imageFolder = "images";
        private const string articleImagesFolder = "article-images";
        private const string userImagesFolder = "user-images";
        #endregion

        #region Ctor
        public ImageHelper(IWebHostEnvironment env)
        {
            _env = env;
            wwwroot = _env.WebRootPath;
        }
        #endregion
        private string ReplaceInvalidChars(string fileName)
        {
            return fileName.Replace("İ", "I")
                 .Replace("ı", "i")
                 .Replace("Ğ", "G")
                 .Replace("ğ", "g")
                 .Replace("Ü", "U")
                 .Replace("ü", "u")
                 .Replace("ş", "s")
                 .Replace("Ş", "S")
                 .Replace("Ö", "O")
                 .Replace("ö", "o")
                 .Replace("Ç", "C")
                 .Replace("ç", "c")
                 .Replace("é", "")
                 .Replace("!", "")
                 .Replace("'", "")
                 .Replace("^", "")
                 .Replace("+", "")
                 .Replace("%", "")
                 .Replace("/", "")
                 .Replace("(", "")
                 .Replace(")", "")
                 .Replace("=", "")
                 .Replace("?", "")
                 .Replace("_", "")
                 .Replace("*", "")
                 .Replace("æ", "")
                 .Replace("ß", "")
                 .Replace("@", "")
                 .Replace("€", "")
                 .Replace("<", "")
                 .Replace(">", "")
                 .Replace("#", "")
                 .Replace("$", "")
                 .Replace("½", "")
                 .Replace("{", "")
                 .Replace("[", "")
                 .Replace("]", "")
                 .Replace("}", "")
                 .Replace(@"\", "")
                 .Replace("|", "")
                 .Replace("~", "")
                 .Replace("¨", "")
                 .Replace(",", "")
                 .Replace(";", "")
                 .Replace("`", "")
                 .Replace(".", "")
                 .Replace(":", "")
                 .Replace(" ", "");
        }


        #region Delete
        public void Delete(string imageName)
        {
            var fileToDelete = $"{wwwroot}{imageName}";

            if (File.Exists(fileToDelete))
            {
                File.Delete(fileToDelete);
            }
        }
        #endregion

        #region Upload
        public async Task<ImageUploadDto> Upload(string name, IFormFile imageFile, ImageType imageType, string? folderName = null)
        {
            folderName ??= imageType == ImageType.User
                ? userImagesFolder
                : articleImagesFolder;

            if (!Directory.Exists($"{wwwroot}/{imageFolder}/{folderName}"))
                Directory.CreateDirectory($"{wwwroot}/{imageFolder}/{folderName}");

            string fileExtension = Path.GetExtension(imageFile.FileName);
            name = ReplaceInvalidChars(name);
            DateTime datetime = DateTime.Now;
            string newFileName = $"{name}_{datetime.Second}{datetime.Minute}{datetime.Millisecond}{fileExtension}";
            var path = Path.Combine($"{wwwroot}/{imageFolder}/{folderName}/", newFileName);

            await using var stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None, 1024 * 1024, false);
            await imageFile.CopyToAsync(stream);
            await stream.FlushAsync();

            return new ImageUploadDto
            {
                FullName = $"/{imageFolder}/{folderName}/{newFileName}"
            };
        }
        #endregion
    }
}
