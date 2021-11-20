using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProITM.Shared;
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
        public Task AddHost(HostModel host)
        {
            throw new NotImplementedException("Implement me");
        }

        [HttpDelete("{hostId}")]
        public Task DeleteHost(string hostId)
        {
            throw new NotImplementedException("Implement me");
        }

        [HttpGet("list")]
        public Task GetHosts()
        {
            throw new NotImplementedException("Implement me");
        }

        [HttpGet("logs/{hostId}")]
        public Task GetHostLogs(string hostId)
        {
            throw new NotImplementedException("Implement me");
        }
    }
}
