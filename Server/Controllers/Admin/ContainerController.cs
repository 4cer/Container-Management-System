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
using ProITM.Server.Utilities;

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
