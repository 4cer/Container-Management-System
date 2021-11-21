using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Docker.DotNet.Models;

namespace ProITM.Server.Controllers
{
    [Authorize(Roles = "User")]
    [ApiController]
    [Route("[controller]")]
    public class ContainerController : ControllerBase
    {
        private Docker.DotNet.DockerClient c;

        [HttpGet]
        public async Task<IEnumerable<ContainerListResponse>> ListContainers()
        {
            throw new NotImplementedException("Implement me");
        }

        [HttpPost]
        public async Task StartContainer(string id)
        {
            throw new NotImplementedException("Implement me");
        }

        [HttpPost]
        public async Task StopContainer(string id)
        {
            throw new NotImplementedException("Implement me");
        }

        [HttpGet]
        public async Task GetContainerStats(string id)
        {
            throw new NotImplementedException("Implement me");
        }

        [HttpPost]
        public async Task CreateContainer(string id)
        {
            throw new NotImplementedException("Implement me");
        }

        [HttpDelete]
        public async Task DeleteContainer(string id)
        {
            throw new NotImplementedException("Implement me");
        }

        [HttpGet]
        public async Task GetContainerLogs(string id)
        {
            throw new NotImplementedException("Implement me");
        }
    }
}
