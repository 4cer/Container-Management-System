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
        public Task<IEnumerable<ContainerListResponse>> ListContainers()
        {
            throw new NotImplementedException("Implement me");
        }

        [HttpPost]
        public Task StartContainer(string id)
        {
            throw new NotImplementedException("Implement me");
        }

        [HttpPost]
        public Task StopContainer(string id)
        {
            throw new NotImplementedException("Implement me");
        }

        [HttpGet]
        public Task GetContainerStats(string id)
        {
            throw new NotImplementedException("Implement me");
        }

        [HttpPost]
        public Task CreateContainer(string id)
        {
            throw new NotImplementedException("Implement me");
        }

        [HttpDelete]
        public Task DeleteContainer(string id)
        {
            throw new NotImplementedException("Implement me");
        }

        [HttpGet]
        public Task GetContainerLogs(string id)
        {
            throw new NotImplementedException("Implement me");
        }
    }
}
