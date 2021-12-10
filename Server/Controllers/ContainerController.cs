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
        private readonly ApplicationDbContext dbContext;
        private readonly UserManager<ApplicationUser> userManager;
        //private static readonly string tmpUri = "http://wido.proitm.tk:52375";

        public ContainerController(ApplicationDbContext _db, UserManager<ApplicationUser> userManager)
        {
            dbContext = _db;
            this.userManager = userManager;
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
                .FirstOrDefaultAsync();

            var containers = userContainers.Take((int)limit);

            return Ok(containers);
        }

        [HttpGet("{containerId}")]
        public async Task<IActionResult> ContainerDetails(string containerId)
        {
            return Ok(GetContainerById(containerId));
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
            var host = await GetLeastBusyHost(model.IsWindows);

            if (string.IsNullOrEmpty(host.URI))
                return NotFound();

            // Construct model
            string userId = User.FindFirst(x => x.Type.Equals(ClaimTypes.NameIdentifier))?.Value;
            var image = await dbContext.Images.SingleOrDefaultAsync(i => i.Id == model.ImageIdC);
            dbContext.Attach(image);
            model.Image = image;

            dbContext.Attach(host);
            var port = new ContainerPortModel() { Id = Guid.NewGuid().ToString(), Port = model.PortNo, Host = host };
            model.Machine = host;
            model.Port = port;
            model.IsRunning = false;

            // Make new instance of DockerClient from URI
            DockerClient dockerClient = new DockerClientConfiguration(new Uri(host.URI)).CreateClient();

            CreateContainerResponse result = await dockerClient.Containers.CreateContainerAsync(new CreateContainerParameters()
            {
                Image = model.Image.Name,
                HostConfig = new HostConfig()
                {
                    DNS = new[] { "8.8.8.8", "8.8.4.4" }
                }
                // TODO bind port
            });

            // TODO pass warnings as list for analysis


            model.Id = result.ID;

            dbContext.Containers.Add(model);

            var user = dbContext.Users.Include(u => u.Containers).FirstOrDefault(u => u.Id == userId);
            dbContext.Attach(user);
            user.Containers.Add(model);

            dbContext.SaveChanges();

            return Ok(result.Warnings);
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


            ContainerModel model = await dbContext.Containers
                .Include(c => c.Port)
                .SingleOrDefaultAsync(c => c.Id == containerId);
            var port = model.Port;
            dbContext.Containers.Attach(model);
            dbContext.Containers.Remove(model);
            dbContext.ContainerPorts.Attach(port);
            dbContext.ContainerPorts.Remove(port);
            dbContext.SaveChanges();

            return Ok();
        }

        [HttpGet("logs/{containerId}/{since}/{tail}")]
        public async Task<IActionResult> GetContainerLogs(string containerId, string since, string tail)
        {
            var dockerClient = GetContainerById(containerId)
                .Machine
                .GetDockerClient();

            var log_stream = await dockerClient.Containers.GetContainerLogsAsync(containerId, true, new ContainerLogsParameters { ShowStdout = true, ShowStderr = true, Timestamps = true, Tail = "50"}, default);
            var log_tuple = (await log_stream.ReadOutputToEndAsync(default)).ToTuple();

            // TODO splice HTML format markup into logs

            return Ok(log_tuple);
        }

        private ContainerModel GetContainerById(string containerId)
        {
            return dbContext.Containers
                .AsNoTracking()
                .Include(c => c.Machine)
                .First(c => c.Id == containerId);
        }

        private async Task<HostModel> GetLeastBusyHost(bool windows)
        {
            // TODO 190 Make it actually check CPU/RAM stats
            var hosts = await dbContext.Hosts
                .AsNoTracking()
                .Where(h => h.IsWindows == windows)
                .ToListAsync();

            HostModel minhost = hosts[0];

            int min = int.MaxValue;
            foreach(var host in hosts)
            {
                var cmp = dbContext.Containers
                    .AsNoTracking()
                    .Where(c => c.Machine.Id == host.Id)
                    .Count();
                if (cmp < min)
                {
                    min = cmp;
                    minhost = host;
                }
            }

            return minhost;
        }
    }
}
