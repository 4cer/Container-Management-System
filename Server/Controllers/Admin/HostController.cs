﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProITM.Server.Data;
using ProITM.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ProITM.Server.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("[controller]")]
    public class HostController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;

        public HostController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        // TODO 176 Implement HostController endpoint methods

        [HttpPost("create")]
        public async Task<IActionResult> AddHost(HostModel host)
        {
            dbContext.Hosts.Add(host);

            return Ok("Added host to databse");
        }

        [HttpDelete("{hostId}")]
        public async Task<IActionResult> DeleteHost(string hostId)
        {
            var host = dbContext.Hosts.First(h => h.Id == hostId );

            dbContext.Hosts.Remove(host);

            return Ok("Deleted successfully");
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetHosts()
        {
            var host = dbContext.Hosts.ToList();

            return Ok(host);
        }

        [HttpGet("logs/{hostId}")]
        public async Task<IActionResult> GetHostLogs(string hostId)
        {
            // TODO decide if necessary

            throw new NotImplementedException("Implement me");
        }
    }
}
