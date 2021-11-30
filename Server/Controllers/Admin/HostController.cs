using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProITM.Server.Data;
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
        private readonly ApplicationDbContext dbContext;
        //whatever

        public HostController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        // TODO 140 Implement HostController endpoint methods

        [HttpPost("create")]
        public async Task<IActionResult> AddHost(HostModel host)
        {
            throw new NotImplementedException("Implement me");
        }

        [HttpDelete("{hostId}")]
        public async Task<IActionResult> DeleteHost(string hostId)
        {
            throw new NotImplementedException("Implement me");
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetHosts()
        {
            throw new NotImplementedException("Implement me");
        }

        [HttpGet("logs/{hostId}")]
        public async Task<IActionResult> GetHostLogs(string hostId)
        {
            throw new NotImplementedException("Implement me");
        }
    }
}
