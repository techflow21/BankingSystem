using BankingSystem.Service.ServiceInterfaces;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;

namespace BankingSystem.Service.ExternalServices
{
    public class ImageUploadService : IImageUploadService
    {
        public async Task<string> PhotoUpload(IFormFile file)
        {
            if (file == null) throw new ArgumentNullException("File is missing or empty");

            string filename = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            string imageFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images");
            Directory.CreateDirectory(imageFolderPath);

            var fullPath = Path.Combine(imageFolderPath, filename);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return Path.Combine("Images", filename);
        }
    }
}
