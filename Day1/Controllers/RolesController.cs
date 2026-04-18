using Day1.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Day1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {

        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;

        public RolesController(RoleManager<IdentityRole>  roleManager,
                               UserManager<IdentityUser> userManager)
        {
            _roleManager = roleManager;

            _userManager = userManager;
        }

        [Authorize(Roles = "Admin")]

        [HttpPost]

        public async Task<IActionResult> CreateRole([FromBody] RoleDto dto)
        {

            if (string.IsNullOrEmpty(dto.RoleName))
                return BadRequest("Rolename is Required");
            //var role = new _roleManager(roleName);
            var exist = await _roleManager.RoleExistsAsync(dto.RoleName);

            if (exist)
                return BadRequest("Role allready exist");

            var role = new IdentityRole(dto.RoleName);
            var result = await _roleManager.CreateAsync(role);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok(result); 
        }

        [HttpPost("assign")]
        public async Task<IActionResult> assignRole([FromBody] AssignRoleDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);

            if (user == null)
                return NotFound("User Not Found");

            var result = await _userManager.AddToRoleAsync(user, dto.RoleName);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok("Role Assiend");
        }



    }
}
