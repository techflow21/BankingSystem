using Microsoft.AspNetCore.Http;

namespace BankingSystem.Service.ServiceInterfaces
{
    public interface IImageUploadService
    {
        Task<string> PhotoUpload(IFormFile file);
    }
}
