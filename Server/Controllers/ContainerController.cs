using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Docker.DotNet.Models;
using ProITM.Server.Data;
using System.Threading;
using Docker.DotNet;

namespace ProITM.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ContainerController : ControllerBase
    {
        // TODO 156 Inject database ApplicationDbContext
        private readonly ApplicationDbContext dbContext;

        public ContainerController(ApplicationDbContext _db)
        {
            // Tu wstrzyknąć zależności
            dbContext = _db;
            // dbContext.Database.EnsureCreated();
        }



        // TODO 144 Implement ContainerController endpoint methods

        [HttpGet("containers/{userId}/{limit}")]
        public async Task<IActionResult> ListContainers(string userId, long limit)
        {
            // TODO Get container list DB, based on user ID
            string URI = "GET ME AN URI";

            // Make new instance of DockerClient from URI
            DockerClient dockerClient = new DockerClientConfiguration(new Uri(URI)).CreateClient();

            //throw new NotImplementedException("Incorrect implementation REDACTED");
            IList<ContainerListResponse> containers = await dockerClient.Containers.ListContainersAsync(new ContainersListParameters() { Limit = limit });
            return Ok(containers);
        }

        [HttpPost("start/{containerId}")]
        public async Task<IActionResult> StartContainer(string containerId)
        {
            // TODO Get container host URI from DB, based on container ID
            string URI = "GET ME AN URI";

            // Make new instance of DockerClient from URI
            DockerClient dockerClient = new DockerClientConfiguration(new Uri(URI)).CreateClient();

            //throw new NotImplementedException("Implement me");
            var start = await dockerClient.Containers.StartContainerAsync(containerId, new ContainerStartParameters());
            if (start == true)
            {
                return Ok("Container started");
            }
            else
            {
                return Problem("Container start - error");
            }
        }

        [HttpPost("stop/{containerId}")]
        public async Task<IActionResult> StopContainer(string containerId)
        {
            // TODO Get container host URI from DB, based on container ID
            string URI = "GET ME AN URI";

            // Make new instance of DockerClient from URI
            DockerClient dockerClient = new DockerClientConfiguration(new Uri(URI)).CreateClient();

            //throw new NotImplementedException("Implement me");
            var stopped = await dockerClient.Containers.StopContainerAsync(containerId, new ContainerStopParameters { WaitBeforeKillSeconds = 30 }, CancellationToken.None);
            if (stopped == true)
            {
                return Ok("Container stopped");
            }
            else
            {
                return Problem("Container stop - error");
            }
        }

        [HttpGet("containers/stats/{containerId}")]
        public async Task<IActionResult> GetContainerStats(string containerId)
        {
            // TODO Get container host URI from DB, based on container ID
            string URI = "GET ME AN URI";

            // Make new instance of DockerClient from URI
            DockerClient dockerClient = new DockerClientConfiguration(new Uri(URI)).CreateClient();

            //throw new NotImplementedException("Implement me");
            var stats = await dockerClient.Containers.GetContainerStatsAsync(containerId, new ContainerStatsParameters { Stream = true }, CancellationToken.None);
            return Ok(stats);
        }

        [HttpPost("containers/create/{isWindows}/{imageId}")]
        public async Task<IActionResult> CreateContainer(bool isWindows, string imageId)
        {
            // TODO Get container host URI by selecting less busy host of given system
            string URI = "GET ME AN URI";

            // Make new instance of DockerClient from URI
            DockerClient dockerClient = new DockerClientConfiguration(new Uri(URI)).CreateClient();

            await dockerClient.Containers.CreateContainerAsync(new CreateContainerParameters()
            {
                Image = "fedora/memcached",
                HostConfig = new HostConfig()
                {
                    DNS = new[] { "8.8.8.8", "8.8.4.4" }
                }
            });
            return Ok("Container created");
            //throw new NotImplementedException("Implement me");
        }

        [HttpDelete("containers/{containerId}")]
        public async Task<IActionResult> DeleteContainer(string containerId)
        {
            // TODO Get container host URI from DB, based on container ID
            string URI = "GET ME AN URI";

            // Make new instance of DockerClient from URI
            DockerClient dockerClient = new DockerClientConfiguration(new Uri(URI)).CreateClient();

            await dockerClient.Containers.RemoveContainerAsync(containerId, new ContainerRemoveParameters { Force = true, RemoveLinks = true, RemoveVolumes = true }, CancellationToken.None);
            return Ok("Container deleted");
            //throw new NotImplementedException("Implement me");
        }

        [HttpGet("containers/logs/{containerId}/{since}/{tail}")]
        public async Task<IActionResult> GetContainerLogs(string containerId, string since, string tail)
        {
            // TODO Get container host URI from DB, based on container ID
            string URI = "GET ME AN URI";

            // Make new instance of DockerClient from URI
            DockerClient dockerClient = new DockerClientConfiguration(new Uri(URI)).CreateClient();

            var logs = await dockerClient.Containers.GetContainerLogsAsync(containerId, true, new ContainerLogsParameters { ShowStdout = true, ShowStderr = true, Since = since, Timestamps = true, Follow = true, Tail = tail }, CancellationToken.None);
            return Ok(logs);
            //throw new NotImplementedException("Implement me");
        }

        //private Docker.DotNet.DockerClient c;
        //private readonly ApplicationDbContext dbContext;

        //public ContainerController(ApplicationDbContext dbContext)
        //{
        //    this.dbContext = dbContext;
        //}


        //// TODO 143 Inject Docker.DotNet.DockerClient

        //// TODO 144 Implement ContainerController endpoint methods

        //[HttpGet("containers")]
        //public async Task<IActionResult> ListContainers()
        //{
        //    throw new NotImplementedException("Implement me");
        //}

        //[HttpPost("start/{containerId}")]
        //public async Task<IActionResult> StartContainer(string containerId)
        //{
        //    throw new NotImplementedException("Implement me");
        //}

        //[HttpPost("stop/{containerId}")]
        //public async Task<IActionResult> StopContainer(string containerId)
        //{
        //    throw new NotImplementedException("Implement me");
        //}

        //[HttpGet("containers/stats/{containerId}")]
        //public async Task<IActionResult> GetContainerStats(string containerId)
        //{
        //    throw new NotImplementedException("Implement me");
        //}

        //[HttpPost("containers/create")]
        //public async Task<IActionResult> CreateContainer()
        //{
        //    throw new NotImplementedException("Implement me");
        //}

        //[HttpDelete("containers/{containerId}")]
        //public async Task<IActionResult> DeleteContainer(string containerId)
        //{
        //    throw new NotImplementedException("Implement me");
        //}

        //[HttpGet("containers/logs/{containerId}")]
        //public async Task<IActionResult> GetContainerLogs(string containerId)
        //{
        //    throw new NotImplementedException("Implement me");
        //}
    }
}
