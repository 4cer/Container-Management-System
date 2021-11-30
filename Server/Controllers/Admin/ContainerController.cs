using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProITM.Server.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProITM.Server.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("[controller]")]
    public class ContainerController
    {
        private readonly ApplicationDbContext dbContext;

        public ContainerController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        // TODO 139 Implement ContainerController endpoint methods

        [HttpGet("manage/{userId}")]
        public async Task<IActionResult> GetUserContainers(string userId)
        {
            throw new NotImplementedException("Implement me");
        }

        [HttpPost("manage/start/{containerId}")]
        public async Task<IActionResult> StartUsersContainer(string containerId)
        {
            throw new NotImplementedException("Implement me");
        }

        [HttpPost("manage/stop/{containerId}")]
        public async Task<IActionResult> StopUsersContainer(string containerId)
        {
            throw new NotImplementedException("Implement me");
        }

        [HttpDelete("manage/{containerId}")]
        public async Task<IActionResult> DeleteUsersContainer(string containerId)
        {
            throw new NotImplementedException("Implement me");
        }
    }
}
