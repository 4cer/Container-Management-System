using Docker.DotNet;
using Docker.DotNet.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProITM.Server.Data;
using ProITM.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using ProITM.Server.Utilities;
using ProITM.Server.Models;
using System.Security.Claims;

namespace ProITM.Server.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("[controller]")]
    public class ContainerController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;

        public ContainerController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet("manage/{userId}/{limit}")]
        public async Task<IActionResult> GetUserContainers(string userId, int limit)
        {
            var userContainers = await dbContext.Users
                .AsNoTracking()
                .Where(u => u.Id == userId)
                .Include(u => u.Containers)
                .ThenInclude(c => c.PortBindings)
                .Select(u => u.Containers)
                //.Include(u => u.Containers)
                .FirstOrDefaultAsync();

            var containers = userContainers.Take(limit);

            return Ok(containers);
        }

        [HttpGet("manage/list/{limit}")]
        public async Task<IActionResult> GetContainers(int limit)
        {
            var users = await dbContext.Users
                .AsNoTracking()
                .Include(u => u.Containers)
                .ThenInclude(c => c.Machine)
                .Include(u => u.Containers)
                .ThenInclude(c => c.Image)
                .Include(u => u.Containers)
                .ThenInclude(c => c.PortBindings)
                .Where(u => u.Containers.Any())
                .ToListAsync();

            List<ContainerModel> containers = new();

            foreach (var user in users)
            {
                foreach (var container in user.Containers)
                {
                    container.OwnerName = user.UserName;
                    containers.Add(container);
                }
            }

            return Ok(containers);

            //return await dbContext.Containers
            //    .AsNoTracking()
            //    .Take(limit)
            //    .ToListAsync();
        }

        [HttpGet("manage/refresh")]
        public async Task<IActionResult> RefreshAllContainers()
        {
            // TODO 298 Refresh all containers
            //string userId = User.FindFirst(x => x.Type.Equals(ClaimTypes.NameIdentifier))?.Value;

            // Get all user containers
            var containers = await dbContext.Users
                //.AsNoTracking()
                .Include(u => u.Containers)
                .ThenInclude(c => c.Machine)
                .Select(u => u.Containers)
                .FirstOrDefaultAsync();

            var groupedContainers = containers.GroupBy(c => c.Machine.Id).ToList();

            foreach (var contGroup in groupedContainers)
            {
                if (contGroup.Any())
                {
                    var client = contGroup.FirstOrDefault().Machine.GetDockerClient();

                    var dcContainers = await client.Containers.ListContainersAsync(new ContainersListParameters { All = true });

                    // TODO 271 anything below is to be written/rewritten

                    // Iterating over the set containing the other set is suboptimal
                    //foreach(var dcc in dcContainers)
                    foreach (var dbc in contGroup)
                    {
                        var container = dcContainers.FirstOrDefault(c => c.ID == dbc.Id);
                        if (container == null)
                        {
                            dbc.State = "Unknown (No reply from Docker API)";
                            continue;
                            //return NotFound();
                        }
                        dbc.IsRunning = (container.State.Equals("running")) ? true : false;
                        dbc.State = container.Status;
                    }
                }
            }

            await dbContext.SaveChangesAsync();

            return Ok(true);
        }

        [HttpGet("manage/{containerId}")]
        public async Task<IActionResult> ContainerDetails(string containerId)
        {
            return Ok(GetContainerById(containerId));
        }

        [HttpPost("manage/start/{containerId}")]
        public async Task<IActionResult> StartUsersContainer(string containerId)
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

        [HttpPost("manage/stop/{containerId}")]
        public async Task<IActionResult> StopUsersContainer(string containerId)
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

        [HttpDelete("manage/{containerId}")]
        public async Task<IActionResult> DeleteUsersContainer(string containerId)
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
                .Include(c => c.PortBindings)
                .SingleOrDefaultAsync(c => c.Id == containerId);

            // Test port deletion
            dbContext.ContainerPorts.AttachRange(model.PortBindings);
            dbContext.ContainerPorts.RemoveRange(model.PortBindings);

            dbContext.Containers.Attach(model);
            dbContext.Containers.Remove(model);
            dbContext.SaveChanges();

            return Ok();
        }

        private ContainerModel GetContainerById(string containerId)
        {
            return dbContext.Containers
                .AsNoTracking()
                .Include(c => c.Machine)
                .First(c => c.Id == containerId);
        }
    }
}
