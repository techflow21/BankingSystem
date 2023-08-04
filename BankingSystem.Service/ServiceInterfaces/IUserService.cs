using BankingSystem.Core.DTOs.Requests;
using BankingSystem.Core.DTOs.Responses;

namespace BankingSystem.Service.ServiceInterfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserResponse>> GetUsersAsync();
        Task<UserResponse> UpdateUserAsync(string id, RegisterRequest request);
        Task<IEnumerable<UserResponse>> SearchUsersAsync(string searchQuery);
        Task<string> AddUserToRoleAsync(AddUserToRoleRequest request);
        Task<bool> DeactivateUserAsync(string id);
        Task<bool> DeleteUserAsync(string id);
    }
}
