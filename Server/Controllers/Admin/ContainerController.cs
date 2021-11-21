using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        [HttpGet("{userId}")]
        public async Task GetUserContainers(string userId)
        {
            throw new NotImplementedException("Implement me");
        }

        [HttpPost("start/{containerId}")]
        public async Task StartUsersContainer(string containerId)
        {
            throw new NotImplementedException("Implement me");
        }

        [HttpPost("stop/{containerId}")]
        public async Task StopUsersContainer(string containerId)
        {
            throw new NotImplementedException("Implement me");
        }

        [HttpDelete("{containerId}")]
        public async Task DeleteUsersContainer(string containerId)
        {
            throw new NotImplementedException("Implement me");
        }
    }
}
