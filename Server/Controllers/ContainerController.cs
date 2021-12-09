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
using ProITM.Shared;
using ProITM.Server.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using ProITM.Server.Utilities;
using Microsoft.AspNetCore.Http;

namespace ProITM.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ContainerController : ControllerBase
    {
        // TODO 156 Inject database ApplicationDbContext
        private readonly ApplicationDbContext dbContext;
        private readonly UserManager<ApplicationUser> userManager;
        private static readonly string tmpUri = "http://wido.proitm.tk:52375";

        public ContainerController(ApplicationDbContext _db, UserManager<ApplicationUser> userManager)
        {
            // Tu wstrzyknąć zależności
            dbContext = _db;
            this.userManager = userManager;
            // dbContext.Database.EnsureCreated();
        }



        // TODO 144 Implement ContainerController endpoint methods

        [HttpGet("containers/{limit}")]
        public async Task<IActionResult> ListContainers(long limit)
        {
            string userId = User.FindFirst(x => x.Type.Equals(ClaimTypes.NameIdentifier))?.Value;

            var userContainers = await dbContext.Users
                .AsNoTracking()
                .Where(u => u.Id == userId)
                .Select(u => u.Containers)
                //.Include(u => u.Containers)
                .FirstOrDefaultAsync();

            var containers = userContainers.Take((int)limit);

            if (!containers.Any())
            {
                return NotFound();
            }
            else
            {
                return Ok(containers);
            }

            //// Get container list DB, based on user ID
            //string URI = "GET ME AN URI";

            //// Make new instance of DockerClient from URI
            //DockerClient dockerClient = new DockerClientConfiguration(new Uri(URI)).CreateClient();

            //IList<ContainerListResponse> containers = await dockerClient.Containers.ListContainersAsync(new ContainersListParameters() { Limit = limit });
            //return Ok(containers);
        }

        [HttpPost("start/{containerId}")]
        public async Task<IActionResult> StartContainer(string containerId)
        {
            var dockerClient = GetContainerById(containerId)
                .Machine
                .GetDockerClient();

            var success = await dockerClient.Containers
                .StartContainerAsync(containerId, new ContainerStartParameters());

            if (success)
            {
                var result = await dbContext.Containers.SingleOrDefaultAsync(c => c.Id == containerId);
                if(result != null)
                {
                    result.IsRunning = true;
                    dbContext.SaveChangesAsync().Wait();
                }

                return Ok();
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            
            //// Get container host URI from DB, based on container ID
            //string URI = "GET ME AN URI";

            //// Make new instance of DockerClient from URI
            //DockerClient dockerClient = new DockerClientConfiguration(new Uri(URI)).CreateClient();

            ////throw new NotImplementedException("Implement me");
            //var start = await dockerClient.Containers.StartContainerAsync(containerId, new ContainerStartParameters());
            //if (start == true)
            //{
            //    return Ok("Container started");
            //}
            //else
            //{
            //    return Problem("Container start - error");
            //}
        }

        [HttpPost("stop/{containerId}")]
        public async Task<IActionResult> StopContainer(string containerId)
        {
            var dockerClient = GetContainerById(containerId)
                .Machine
                .GetDockerClient();

            var stopped = await dockerClient.Containers
                .StopContainerAsync(containerId, new ContainerStopParameters()
                {
                    WaitBeforeKillSeconds = 30
                });

            if (stopped)
            {
                var result = await dbContext.Containers.SingleOrDefaultAsync(c => c.Id == containerId);
                if (result != null)
                {
                    result.IsRunning = false;
                    dbContext.SaveChangesAsync().Wait();
                }

                return Ok();
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            //// TODO Get container host URI from DB, based on container ID
            //string URI = "GET ME AN URI";

            //// Make new instance of DockerClient from URI
            //DockerClient dockerClient = new DockerClientConfiguration(new Uri(URI)).CreateClient();

            //var stopped = await dockerClient.Containers.StopContainerAsync(containerId, new ContainerStopParameters { WaitBeforeKillSeconds = 30 }, CancellationToken.None);
            //if (stopped == true)
            //{
            //    return Ok("Container stopped");
            //}
            //else
            //{
            //    return Problem("Container stop - error");
            //}
        }

        [HttpGet("stats/{containerId}")]
        public async Task<IActionResult> GetContainerStats(string containerId)
        {
            //// TODO Get container host URI from DB, based on container ID
            //string URI = "GET ME AN URI";

            //// Make new instance of DockerClient from URI
            //DockerClient dockerClient = new DockerClientConfiguration(new Uri(URI)).CreateClient();
            
            var dockerClient = GetContainerById(containerId)
                .Machine
                .GetDockerClient();

            var stats = await dockerClient.Containers.GetContainerStatsAsync(containerId, new ContainerStatsParameters { Stream = true }, CancellationToken.None);
            return Ok(stats);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateContainer(ContainerModel model)
        {

            // TODO Get container host URI by selecting least busy host of given system
            //string URI = "GET ME AN URI";
            string URI = tmpUri;

            // Make new instance of DockerClient from URI
            DockerClient dockerClient = new DockerClientConfiguration(new Uri(URI)).CreateClient();

            CreateContainerResponse result = await dockerClient.Containers.CreateContainerAsync(new CreateContainerParameters()
            {
                Image = model.Image.Name,
                HostConfig = new HostConfig()
                {
                    DNS = new[] { "8.8.8.8", "8.8.4.4" }
                }
            });

            // TODO pass warnings as list for analysis

            // Construct model
            string userId = User.FindFirst(x => x.Type.Equals(ClaimTypes.NameIdentifier))?.Value;
            var image = await dbContext.Images.SingleOrDefaultAsync(i => i.Id == "ngineex");
            dbContext.Attach(image);
            model.Image = image;

            var host = new HostModel() { Id = "hosto" };
            dbContext.Attach(host);
            var port = new ContainerPortModel() { Id = Guid.NewGuid().ToString(), Port = 2137, Host = host };
            model.Machine = host;
            model.Port = port;
            model.IsRunning = false;

            model.Id = result.ID;

            dbContext.Containers.Add(model);

            var user = dbContext.Users.Include(u => u.Containers).FirstOrDefault(u => u.Id == userId);
            dbContext.Attach(user);
            user.Containers.Add(model);

            dbContext.SaveChanges();


            return Ok("Container created");
        }

        [HttpDelete("{containerId}")]
        public async Task<IActionResult> DeleteContainer(string containerId)
        {
            var dockerClient = GetContainerById(containerId)
                .Machine
                .GetDockerClient();

            await dockerClient.Containers
                .StopContainerAsync(containerId, new ContainerStopParameters()
                {
                    WaitBeforeKillSeconds = 30
                });

            await dockerClient.Containers
                .RemoveContainerAsync(containerId, new ContainerRemoveParameters());


            ContainerModel model = new() { Id = containerId };
            dbContext.Containers.Attach(model);
            dbContext.Containers.Remove(model);
            // TODO Remove container port
            dbContext.SaveChanges();

            return Ok();

            //// TODO Get container host URI from DB, based on container ID
            //string URI = "GET ME AN URI";

            //// Make new instance of DockerClient from URI
            //DockerClient dockerClient = new DockerClientConfiguration(new Uri(URI)).CreateClient();

            //await dockerClient.Containers.RemoveContainerAsync(containerId, new ContainerRemoveParameters { Force = true, RemoveLinks = true, RemoveVolumes = true }, CancellationToken.None);
            //return Ok("Container deleted");
        }

        [HttpGet("logs/{containerId}/{since}/{tail}")]
        public async Task<IActionResult> GetContainerLogs(string containerId, string since, string tail)
        {
            //// TODO Get container host URI from DB, based on container ID
            //string URI = "GET ME AN URI";

            //// Make new instance of DockerClient from URI
            //DockerClient dockerClient = new DockerClientConfiguration(new Uri(URI)).CreateClient();

            var dockerClient = GetContainerById(containerId)
                .Machine
                .GetDockerClient();

            var logs = await dockerClient.Containers.GetContainerLogsAsync(containerId, true, new ContainerLogsParameters { ShowStdout = true, ShowStderr = true, Since = since, Timestamps = true, Follow = true, Tail = tail }, CancellationToken.None);
            return Ok(logs);
        }

        private ContainerModel GetContainerById(string containerId)
        {
            return dbContext.Containers
                .AsNoTracking()
                .Include(c => c.Machine)
                .First(c => c.Id == containerId);
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
