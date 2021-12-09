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
                .Select(u => u.Containers)
                //.Include(u => u.Containers)
                .FirstOrDefaultAsync();

            var containers = userContainers.Take(limit);

            if(!containers.Any())
            {
                return NotFound();
            } else
            {
                return Ok(containers);
            }
        }

        [HttpGet("manage/list/{limit}")]
        public async Task<List<ContainerModel>> GetContainers(int limit)
        {
            return await dbContext.Containers
                .AsNoTracking()
                .Take(limit)
                .ToListAsync();
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
                .RemoveContainerAsync(containerId, new ContainerRemoveParameters());
            
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
