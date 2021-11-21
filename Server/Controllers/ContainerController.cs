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

        // TODO Inject database Container context
            // TODO Implement ContainerContext
        // TODO Inject Docker.DotNet.DockerClient

        // TODO Implement ContainerController endpoint methods

        [HttpGet]
        public async Task<IActionResult> ListContainers()
        {
            throw new NotImplementedException("Implement me");
        }

        [HttpPost]
        public async Task<IActionResult> StartContainer(string id)
        {
            throw new NotImplementedException("Implement me");
        }

        [HttpPost]
        public async Task<IActionResult> StopContainer(string id)
        {
            throw new NotImplementedException("Implement me");
        }

        [HttpGet]
        public async Task<IActionResult> GetContainerStats(string id)
        {
            throw new NotImplementedException("Implement me");
        }

        [HttpPost]
        public async Task<IActionResult> CreateContainer(string id)
        {
            throw new NotImplementedException("Implement me");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteContainer(string id)
        {
            throw new NotImplementedException("Implement me");
        }

        [HttpGet]
        public async Task<IActionResult> GetContainerLogs(string id)
        {
            throw new NotImplementedException("Implement me");
        }
    }
}
