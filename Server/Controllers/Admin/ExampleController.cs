using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProITM.Server.Models;
using ProITM.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ProITM.Server.Controllers.Admin
{
    // Authorize attribute may also be given without specifing role
    // making it available to all logged in users
    [Authorize(Roles = "Admin")]
    // Controler listens on "{METHOD}:{URI}/ObsoleteExample/*" route,
    // where {METHOD} is Http request method, {URI} is eg ProjectLocalIP:Port or
    // proitm.tk, * specifies controller route
    // example GET:proitm.tk/ObsoleteExample/Adminroleid
    [Route("[controller]")]
    [ApiController]
    public class ExampleController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;

        // Inject IdentityManager instances
        // Other controllers may instead use a data context
        public ExampleController(RoleManager<IdentityRole> roleManager,
            UserManager<Models.ApplicationUser> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        [HttpGet("myUserId")]
        public async Task<IActionResult> GetMyId()
        {
            string useruno = User.FindFirst(x => x.Type.Equals(ClaimTypes.NameIdentifier))?.Value;
            var user = await userManager.FindByIdAsync(useruno);
            UserModel usrs = new()
            {
                Id = user.Id
            };
            return Ok(usrs);
        }

        [HttpGet("Adminroleid")]
        public async Task<IActionResult> GetAdminRoleId()
        {
            // Call injected RoleManager<IdentityRole> instance for identity information
            var adminRole = await roleManager.FindByNameAsync("Admin");

            if(adminRole == null)
            {
                // If controller experienced a problem
                return NotFound();
            }

            // If controller got what was expected
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

        // Obsolete below

        [HttpGet]
        public async Task<IActionResult> GetGroups()
        {
            var roles = roleManager.Roles.ToArray();
            //System.Diagnostics.Debug.WriteLine("GroupController.cs: " + roles.ToString());

            return Ok(roles);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetGroup(string id)
        {
            var group = roleManager.Roles.FirstOrDefault(g => g.Id.Equals(id));
            if (group == null)
                return NotFound("No group found.");

            return Ok(group);
        }

        [HttpPost]
        public async Task<IActionResult> CreateGroup(Group group)
        {
            IdentityRole identityRole = new()
            {
                Name = group.Name
            };

            IdentityResult result = await roleManager.CreateAsync(identityRole);

            if(result.Succeeded)
            {
                return Ok();
            }

            return Problem(detail: "Could not create identity group. Check if name is unique.");
        }

        [HttpPost("Edit")]
        public async Task<IActionResult> EditGroup(Group group)
        {
            var role = await roleManager.FindByIdAsync(group.Id);

            role.Name = group.Name;

            IdentityResult result = await roleManager.UpdateAsync(role);

            if (result.Succeeded)
            {
                return Ok();
            }

            return Problem(detail: "Could not update identity group. Check if name is unique.");
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteGroup(string id)
        {
            var role = await roleManager.FindByIdAsync(id);

            IdentityResult result = await roleManager.DeleteAsync(role);

            if (result.Succeeded)
            {
                return Ok();
            }

            return Problem();
        }

        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> GetUsersInGroup(string id)
        {
            var role = await roleManager.FindByIdAsync(id);

            if(role == null)
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

            if(role == null)
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
