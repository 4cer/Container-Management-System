using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProITM.Server.Models;

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

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            throw new NotImplementedException("AdministrationController.GetUsers()");
        }

        [HttpGet("user/{Id}")]
        public async Task<IActionResult> GetUser(string Id)
        {
            throw new NotImplementedException("AdministrationController.GetUser(string Id)");
        }

        [HttpDelete("user/{Id}")]
        public async Task<IActionResult> DeleteUser(string Id)
        {
            throw new NotImplementedException("AdministrationController.DeleteUser(string Id)");
        }

        [HttpPut("user/promote/{Id}")]
        public async Task<IActionResult> PromoteUser(string Id)
        {
            throw new NotImplementedException("AdministrationController.PromoteUser(string Id)");
        }

        [HttpPut("user/demote/{Id}")]
        public async Task<IActionResult> DemoteUser(string Id)
        {
            throw new NotImplementedException("AdministrationController.DemoteUser(string Id)");
        }
    }
}
