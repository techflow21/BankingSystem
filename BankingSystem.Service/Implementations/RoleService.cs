using AutoMapper;
using BankingSystem.Core.DTOs.Requests;
using BankingSystem.Service.ServiceInterfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BankingSystem.Service.Implementations
{
    public class RoleService : IRoleService
    {
        private readonly IMapper _mapper;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleService(IMapper mapper, RoleManager<IdentityRole> roleManager)
        {
            _mapper = mapper;
            _roleManager = roleManager;
        }

        public async Task<RoleRequest> CreateRole(RoleRequest model)
        {
            var role = _mapper.Map<IdentityRole>(model.Name.ToLower());
            var createdRole = await _roleManager.CreateAsync(role);
            var roleDto = _mapper.Map<RoleRequest>(createdRole);
            return roleDto;
        }

        public async Task<RoleRequest> UpdateRole(string roleId, RoleRequest model)
        {
            var role = await _roleManager.FindByIdAsync(roleId) ?? throw new ApplicationException("Role not found");
            role.Name = model.Name.ToLower();

            var updatedRole = await _roleManager.UpdateAsync(role);
            var roleDto = _mapper.Map<RoleRequest>(updatedRole);
            return roleDto;
        }

        public async Task DeleteRole(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId) ?? throw new ApplicationException("Role not found");
            await _roleManager.DeleteAsync(role);
        }

        public async Task<IEnumerable<RoleRequest>> GetRolesAsync()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            var roleDtos = _mapper.Map<IEnumerable<RoleRequest>>(roles);
            return roleDtos;
        }

        public async Task<RoleRequest> GetRoleById(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
                throw new ApplicationException("Role not found");

            var roleDto = _mapper.Map<RoleRequest>(role);
            return roleDto;
        }
    }
}
