using AutoMapper;
using BankingSystem.Core.DTOs.Requests;
using BankingSystem.Core.DTOs.Responses;
using BankingSystem.Core.Entities;
using BankingSystem.Infrastructure.GenericRepository;
using BankingSystem.Service.ServiceInterfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace BankingSystem.Service.Implementations
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UserService> _logger;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IImageUploadService _imageUploadService;

        public UserService(ILogger<UserService> logger, IUnitOfWork unitOfWork, IMapper mapper, UserManager<User> userManager, IImageUploadService imageUploadService, RoleManager<IdentityRole> roleManager)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
            _imageUploadService = imageUploadService;
            _roleManager = roleManager;
        }

        public Task<IEnumerable<UserResponse>> GetUsersAsync()
        {
            var users = _userManager.Users;
            var registeredUsers = _mapper.Map<IEnumerable<UserResponse>>(users);
            return Task.FromResult(registeredUsers);
        }

        public async Task<UserResponse> UpdateUserAsync(string id, RegisterRequest request)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return null;
            }
            _mapper.Map(request, user);

            if (request.Image != null)
            {
                var imageUrl = await _imageUploadService.PhotoUpload(request.Image);
                user.ImageUrl = imageUrl;
                await _userManager.UpdateAsync(user);
            }
            await _unitOfWork.SaveChangesAsync();
            var updatedUserDto = _mapper.Map<UserResponse>(user);
            return updatedUserDto;
        }


        public async Task<bool> DeleteUserAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return false;
            }
            await _userManager.DeleteAsync(user);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }


        public async Task<bool> DeactivateUserAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return false;
            }
            user.IsActive = false;
            await _unitOfWork.SaveChangesAsync();
            return true;
        }


        public Task<IEnumerable<UserResponse>> SearchUsersAsync(string searchQuery)
        {
            var users = _userManager.Users;
            var searchedUsers = users.Where(user =>
                user.UserName.Contains(searchQuery) ||
                user.FirstName.Contains(searchQuery) ||
                user.LastName.Contains(searchQuery)
            );

            var userDtos = _mapper.Map<IEnumerable<UserResponse>>(searchedUsers);
            return Task.FromResult(userDtos);
        }


        public async Task<string> AddUserToRoleAsync(AddUserToRoleRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName.ToLower());
            if (user == null)
            {
                return "User not found";
            }

            var roleExists = await _roleManager.RoleExistsAsync(request.RoleName.ToLower());

            if (!roleExists)
            {
                return "Role not found";
            }
            await _userManager.AddToRoleAsync(user, request.RoleName);
            await _unitOfWork.SaveChangesAsync();
            return $"{user.UserName} was assigned '{request.RoleName} Role'";
        }
    }
}
