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

        [HttpGet("Users")]
        public async Task<IActionResult> GetUsers()
        {
            var users = userManager.Users;

            if (users == null)
                return NotFound("Administration:GET:GetUsers(): User list not found");

            var usersDigested = new List<UserModel>();

            foreach (var user in users)
            {
                bool isAdmin = await userManager.IsInRoleAsync(user, "Admin");

                usersDigested.Add(new UserModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    EmailConfirmed = user.EmailConfirmed,
                    IsAdmin = isAdmin
                });
            }

            return Ok(usersDigested);
        }

        [HttpGet("Users/{Id}")]
        public async Task<IActionResult> GetUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);

            // Error out if user not found?

            if (user == null)
                return NotFound("Administration:GET:GetUser(string id): User not found");

            return Ok(user);
        }

        [HttpPost("Users/Edit")]
        public async Task<IActionResult> EditUser(UserModel user)
        {
            var userEdit = await userManager.FindByIdAsync(user.Id);

            userEdit.UserName = user.UserName;
            userEdit.Email = user.Email;
            userEdit.EmailConfirmed = user.EmailConfirmed;

            IdentityResult result = await userManager.UpdateAsync(userEdit);

            if (result.Succeeded)
            {
                return Ok();
            }

            return Problem("Administration:DELETE:EditUser(UserModel user): Edition unsuccessful");
        }

        [HttpDelete("Users/{Id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);

            // Error out if user not found?

            IdentityResult result = await userManager.DeleteAsync(user);

            if (result.Succeeded)
                return Ok();

            return Problem("Administration:DELETE:DeleteUser(string id): Deletion unsuccessful");
        }

        [HttpPost("Users/Promote/{Id}")]
        public async Task<IActionResult> PromoteUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);

            // Error out if user not found?

            
            IdentityResult result = await userManager.AddToRoleAsync(user, "Admin");

            if (result.Succeeded)
                return Ok();

            return Problem("Administration:PUT:PromoteUser(string id): Could not promote user");
        }

        [HttpPost("Users/Demote/{Id}")]
        public async Task<IActionResult> DemoteUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);

            // Error out if user not found?

            // Guard against demoting only admin in system
            var numInRole = (await userManager.GetUsersInRoleAsync("Admin")).Count;
            if (numInRole < 2)
                return Problem("Administration:PUT:DemoteUser(string id): Cannot demote only admin in system");

            IdentityResult result = await userManager.RemoveFromRoleAsync(user, "Admin");

            if (result.Succeeded)
                return Ok();

            return Problem("Administration:PUT:DemoteUser(string id): Could not demote user");
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

        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> GetUsersInGroup(string id)
        {
            var role = await roleManager.FindByIdAsync(id);

            if (role == null)
            {
                return NotFound();
            }

            var users = new List<UserInRole>();

            foreach (var user in userManager.Users)
            {
                var userInRole = new UserInRole
                {
                    Id = user.Id,
                    UserName = user.UserName
                };

                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    userInRole.IsSelected = true;
                }
                users.Add(userInRole);
            }

            return Ok(users);
        }

        [HttpPost("Edit/{id}")]
        public async Task<IActionResult> PostUsersInGroup(List<UserInRole> usersInRole, string id)
        {
            var role = await roleManager.FindByIdAsync(id);

            if (role == null)
            {
                return NotFound();
            }

            foreach (var user in usersInRole)
            {
                var usr = await userManager.FindByIdAsync(user.Id);
                bool nowInRole = await userManager.IsInRoleAsync(usr, role.Name);

                if (nowInRole && !user.IsSelected)
                {
                    await userManager.RemoveFromRoleAsync(usr, role.Name);
                }
                if (!nowInRole && user.IsSelected)
                {
                    await userManager.AddToRoleAsync(usr, role.Name);
                }
            }

            return Ok();
        }
    }
}
