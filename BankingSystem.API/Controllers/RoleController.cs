using BankingSystem.Core.DTOs.Requests;
using BankingSystem.Service.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BankingSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;
        private readonly ILogger<RoleController> _logger;

        public RoleController(IRoleService roleService, ILogger<RoleController> logger)
        {
            _roleService = roleService;
            _logger = logger;
        }


        [HttpGet("get-roles")]
        [SwaggerOperation(Summary = "Get all roles")]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns all roles", typeof(IEnumerable<RoleRequest>))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error.")]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await _roleService.GetRolesAsync();
            _logger.LogInformation("Fetching all roles");
            return Ok(roles);
        }


        [HttpPost("create-role")]
        [SwaggerOperation(Summary = "Create a new role")]
        [SwaggerResponse(StatusCodes.Status201Created, "Role created", typeof(RoleRequest))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid role data.")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error.")]
        public async Task<IActionResult> CreateRole(RoleRequest model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid role data");
                return BadRequest(ModelState);
            }

            var createdRole = await _roleService.CreateRole(model);
            _logger.LogInformation("Role created");
            return CreatedAtAction(nameof(GetRoleById), createdRole);
        }


        [HttpPut("update-role/{Id}")]
        [SwaggerOperation(Summary = "Update an existing role")]
        [SwaggerResponse(StatusCodes.Status200OK, "Role updated", typeof(RoleRequest))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid role data.")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error.")]
        public async Task<IActionResult> UpdateRole(string roleId, RoleRequest model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid role data");
                return BadRequest(ModelState);
            }

            var updatedRole = await _roleService.UpdateRole(roleId, model);
            _logger.LogInformation("Role updated");
            return Ok(updatedRole);
        }


        [HttpDelete("delete-role/{Id}")]
        [SwaggerOperation(Summary = "Delete a role by ID")]
        [SwaggerResponse(StatusCodes.Status200OK, "Role deleted")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error.")]
        public async Task<IActionResult> DeleteRole(string roleId)
        {
            await _roleService.DeleteRole(roleId);
            _logger.LogInformation("Role deleted");
            return Ok();
        }


        [HttpGet("get-role/{Id}")]
        [SwaggerOperation(Summary = "Get a role by ID")]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns the role", typeof(RoleRequest))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid role ID.")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error.")]
        public async Task<IActionResult> GetRoleById(string roleId)
        {
            var role = await _roleService.GetRoleById(roleId);
            _logger.LogInformation("Fetching role by ID");
            return Ok(role);
        }
    }
}
