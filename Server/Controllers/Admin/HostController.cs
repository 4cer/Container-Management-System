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
    [Route("api/[controller]")]
    public class HostController : ControllerBase
    {
        private List<HostModel> Hosts = new()
        {
            new HostModel() { Id = "0", DisplayName = "name", IsWindows = true, IP = "1.1.1.1", Port = 6969, URI = "www.dupa.com" },
            new HostModel() { Id = "1", DisplayName = "name", IsWindows = true, IP = "1.1.1.1", Port = 6969, URI = "www.dupa.com" },
            new HostModel() { Id = "2", DisplayName = "name", IsWindows = false, IP = "1.1.1.1", Port = 6969, URI = "www.dupa.com" },
            new HostModel() { Id = "3", DisplayName = "name", IsWindows = true, IP = "1.1.1.1", Port = 6969, URI = "www.dupa.com" },
            new HostModel() { Id = "4", DisplayName = "name", IsWindows = false, IP = "1.1.1.1", Port = 6969, URI = "www.dupa.com" },
            new HostModel() { Id = "5", DisplayName = "name", IsWindows = true, IP = "1.1.1.1", Port = 6969, URI = "www.dupa.com" },
            new HostModel() { Id = "6", DisplayName = "name", IsWindows = true, IP = "1.1.1.1", Port = 6969, URI = "www.dupa.com" },
            new HostModel() { Id = "7", DisplayName = "name", IsWindows = false, IP = "1.1.1.1", Port = 6969, URI = "www.dupa.com" },
            new HostModel() { Id = "8", DisplayName = "name", IsWindows = true, IP = "1.1.1.1", Port = 6969, URI = "www.dupa.com" },
            new HostModel() { Id = "9", DisplayName = "name", IsWindows = true, IP = "1.1.1.1", Port = 6969, URI = "www.dupa.com" }
        };

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
            var hosts = dbContext.Hosts.ToList();

            return Ok(Hosts);
        }

        [HttpGet("logs/{hostId}")]
        public async Task<IActionResult> GetHostLogs(string hostId)
        {
            // TODO decide if necessary

            throw new NotImplementedException("Implement me");
        }
    }
}
