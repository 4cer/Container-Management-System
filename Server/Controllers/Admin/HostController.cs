using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProITM.Server.Data;
using ProITM.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProITM.Server.Controllers.Admin
{
    //[Authorize(Roles = "Admin")]
    [ApiController]
    [Route("[controller]")]
    public class HostController : ControllerBase
    {
        /*
        private static List<HostModel> Hosts = new()
        {
            new HostModel() { Id = "0", DisplayName = "name", IsWindows = true, IP = "1.1.1.1", Port = 6969, URI = "www.dupa.com" },
            new HostModel() { Id = "1", DisplayName = "name", IsWindows = true, IP = "1.1.1.1", Port = 6969, URI = "www.dupa.com" },
            new HostModel() { Id = "2", DisplayName = "name", IsWindows = false, IP = "1.1.1.1", Port = 6969, URI = "www.dupa.com" },
            new HostModel() { Id = "3", DisplayName = "name", IsWindows = true, IP = "1.1.1.1", Port = 6969, URI = "www.dupa.com" }
        };
         */

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
            await dbContext.SaveChangesAsync();
            return Ok("Added host to databse");
        }

        [HttpPut("edit")]
        public async Task<IActionResult> EditHost(HostModel host)
        {
            var foundHost = await dbContext.Hosts
                .FirstAsync(h => h.Id == host.Id);

            if (foundHost == null) return BadRequest();

            foundHost.DisplayName = host.DisplayName;
            foundHost.IsWindows = host.IsWindows;
            foundHost.IP = host.IP;
            foundHost.Port = host.Port;
            foundHost.URI = host.URI;

            if (await dbContext.SaveChangesAsync() == 1)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpDelete("{hostId}")]
        public async Task<IActionResult> DeleteHost(string hostId)
        {
            var host = dbContext.Hosts.First(h => h.Id == hostId);
            dbContext.Hosts.Remove(host);
            await dbContext.SaveChangesAsync();

            return Ok("Deleted successfully");
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetHosts()
        {
            var hosts = dbContext.Hosts.ToList();
            return Ok(hosts);
        }

        [HttpGet("{hostId}")]
        public async Task<IActionResult> HostDetails(string hostId)
        {
            return Ok(dbContext.Hosts
                .AsNoTracking()
                .SingleOrDefaultAsync(h => h.Id == hostId));
        }

        [HttpGet("logs/{hostId}")]
        public async Task<IActionResult> GetHostLogs(string hostId)
        {
            // TODO decide if necessary

            throw new NotImplementedException("Implement me");
        }
    }
}
