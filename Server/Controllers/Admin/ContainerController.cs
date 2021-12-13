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

        private ContainerModel GetContainerById(string containerId)
        {
            return dbContext.Containers
                .AsNoTracking()
                .Include(c => c.Machine)
                .First(c => c.Id == containerId);
        }
    }
}
