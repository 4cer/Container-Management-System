using Docker.DotNet;
using Docker.DotNet.Models;
using Microsoft.AspNetCore.Authorization;
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

namespace ProITM.Server.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("[controller]")]
    public class ContainerController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly DockerClient dockerClient;

        public ContainerController(ApplicationDbContext dbContext, DockerClient dockerClient)
        {
            this.dbContext = dbContext;
            this.dockerClient = dockerClient;
        }

        [HttpGet("manage/{userId}/{limit}")]
        public async Task<List<ContainerModel>> GetUserContainers(string userId, int limit)
        {
            return await dbContext.Containers
                .AsNoTracking()
                .Include(c => c.Owner)
                .Where(c => c.Owner.Id == userId)
                .Take(limit)
                .ToListAsync();
        }

        [HttpGet("manage/list/{limit}")]
        public async Task<List<ContainerModel>> GetContainers(int limit)
        {
            return await dbContext.Containers.Take(limit).ToListAsync();
        }

        [HttpPost("manage/start/{containerId}")]
        public async Task<IActionResult> StartUsersContainer(string containerId)
        {
            var success = await dockerClient.Containers
                .StartContainerAsync(containerId, new ContainerStartParameters());

            if (success)
            {
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
            var stopped = await dockerClient.Containers
                .StopContainerAsync(containerId, new ContainerStopParameters()
                {
                    WaitBeforeKillSeconds = 30
                });

            if (stopped)
            {
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
            await dockerClient.Containers
                .RemoveContainerAsync(containerId, new ContainerRemoveParameters());

            return Ok();
        }
    }
}
