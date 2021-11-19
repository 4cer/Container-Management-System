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
    public class HostController
    {
        [HttpPost]
        public Task AddHost()
        {
            throw new NotImplementedException("Implement me");
        }

        [HttpDelete]
        public Task DeleteHost()
        {
            throw new NotImplementedException("Implement me");
        }

        [HttpGet]
        public Task GetHosts()
        {
            throw new NotImplementedException("Implement me");
        }

        [HttpGet]
        public Task GetHostLogs(string hostId)
        {
            throw new NotImplementedException("Implement me");
        }
    }
}
