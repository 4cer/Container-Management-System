using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProITM.Server.Models;
using ProITM.Shared;

namespace ProITM.Server.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    [Route("[controller]")]
    [ApiController]
    public class AdministrationController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;

        public AdministrationController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        [HttpGet("Adminroleid")]
        public async Task<IActionResult> GetAdminRoleId()
        {
            var adminRole = await roleManager.FindByNameAsync("Admin");

            if (adminRole == null)
            {
                return NotFound();
            }

            return Ok(adminRole);
        }

        [HttpGet("Adminroleusers")]
        public async Task<IActionResult> GetAdmins()
        {
            var role = await roleManager.FindByNameAsync("Admin");

            if (role == null)
            {
                return NotFound();
            }

            var users = new List<UserModel>();

            foreach (var user in userManager.Users)
            {
                var userModel = new UserModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    EmailConfirmed = user.EmailConfirmed
                };

                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    users.Add(userModel);
                }

            }

            return Ok(users);
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = userManager.Users;

            if (users == null)
                return NotFound("Administration:GET:GetUsers(): User list not found");

            return Ok(users);
        }

        [HttpGet("user/{Id}")]
        public async Task<IActionResult> GetUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);

            // TODO Error out if user not found?

            if (user == null)
                return NotFound("Administration:GET:GetUser(string id): User not found");

            return Ok(user);
        }

        [HttpDelete("user/{Id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);

            // TODO Error out if user not found?

            IdentityResult result = await userManager.DeleteAsync(user);

            if (result.Succeeded)
                return Ok();

            return Problem("Administration:DELETE:DeleteUser(string id): Deletion unsuccessful");
        }

        [HttpPut("user/promote/{Id}")]
        public async Task<IActionResult> PromoteUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);

            // TODO Error out if user/group not found?

            
            IdentityResult result = await userManager.AddToRoleAsync(user, "Admin");

            if (result.Succeeded)
                return Ok();

            return Problem("Administration:PUT:PromoteUser(string id): Could not promote user");
        }

        [HttpPut("user/demote/{Id}")]
        public async Task<IActionResult> DemoteUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);

            // TODO Error out if user/group not found?


            IdentityResult result = await userManager.RemoveFromRoleAsync(user, "Admin");

            if (result.Succeeded)
                return Ok();

            return Problem("Administration:PUT:DemoteUser(string id): Could not demote user");
        }
    }
}
