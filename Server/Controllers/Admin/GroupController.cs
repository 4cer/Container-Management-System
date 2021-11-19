using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProITM.Server.Models;
using ProITM.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProITM.Server.Controllers.Admin
{
    //[Authorize(Roles = "Admin")]
    [Route("[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;

        public GroupController(RoleManager<IdentityRole> roleManager,
            UserManager<Models.ApplicationUser> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetGroups()
        {
            var roles = roleManager.Roles.ToArray();
            System.Diagnostics.Debug.WriteLine("GroupController.cs: " + roles.ToString());

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
            IdentityRole identityRole = new IdentityRole
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

            var users = new List<UserInRole>();

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
