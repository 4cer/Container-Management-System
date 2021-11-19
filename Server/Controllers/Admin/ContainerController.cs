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
        [HttpGet]
        public Task GetUserContainers(string user)
        {
            throw new NotImplementedException("Implement me");
        }

        [HttpPost]
        public Task StartUsersContainer(string container)
        {
            throw new NotImplementedException("Implement me");
        }

        [HttpPost]
        public Task StopUsersContainer(string container)
        {
            throw new NotImplementedException("Implement me");
        }

        [HttpDelete]
        public Task DeleteUsersContainer(string container)
        {
            throw new NotImplementedException("Implement me");
        }
    }
}
