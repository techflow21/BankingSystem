using BankingSystem.Core.DTOs.Requests;

namespace BankingSystem.Service.ServiceInterfaces
{
    public interface IRoleService
    {
        Task<RoleRequest> CreateRole(RoleRequest request);
        Task<RoleRequest> UpdateRole(string roleId, RoleRequest model);
        Task DeleteRole(string roleId);
        Task<IEnumerable<RoleRequest>> GetRolesAsync();
        Task<RoleRequest> GetRoleById(string roleId);
    }
}
