using BankingSystem.Core.DTOs.Requests;
using BankingSystem.Service.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BankingSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }


        [HttpGet("get-users")]
        [SwaggerOperation(Summary = "get users")]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns all registered users")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error.")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userService.GetUsersAsync();
            if (users == null)
            {
                return BadRequest();
            }
            return Ok(users);
        }


        [HttpGet("search-users")]
        [SwaggerOperation(Summary = "search users")]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns searched users")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error.")]
        public async Task<IActionResult> SearchUsers(string searchTerm)
        {
            var searchedUsers = await _userService.SearchUsersAsync(searchTerm);
            if (searchedUsers == null)
            {
                return BadRequest();
            }
            return Ok(searchedUsers);
        }


        [HttpPut("update-user/{id}")]
        [SwaggerOperation(Summary = "Update user details")]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns the updated user")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "User not found.")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error.")]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] RegisterRequest request)
        {
            var updatedUser = await _userService.UpdateUserAsync(id, request);
            if (updatedUser == null)
            {
                return NotFound();
            }
            return Ok(updatedUser);
        }


        [HttpDelete("delete-user/{id}")]
        [SwaggerOperation(Summary = "Delete a user")]
        [SwaggerResponse(StatusCodes.Status200OK, "User deleted successfully.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "User not found.")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error.")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var result = await _userService.DeleteUserAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return Ok("User deleted successfully.");
        }


        [HttpPut("deactivate-user/{id}")]
        [SwaggerOperation(Summary = "Deactivate a user")]
        [SwaggerResponse(StatusCodes.Status200OK, "User deactivated successfully.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "User not found.")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error.")]
        public async Task<IActionResult> DeactivateUser(string id)
        {
            var result = await _userService.DeactivateUserAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return Ok("User deactivated successfully.");
        }


        [HttpPost("add-user-to-role")]
        [SwaggerOperation(Summary = "Add a user to a role")]
        [SwaggerResponse(StatusCodes.Status200OK, "User assigned to role successfully.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "User or role not found.")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error.")]
        public async Task<IActionResult> AddUserToRole([FromBody] AddUserToRoleRequest request)
        {
            var result = await _userService.AddUserToRoleAsync(request);
            if (result.Contains("not found"))
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
