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
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using System.IO;

namespace ProITM.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ContainerController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly UserManager<ApplicationUser> userManager;

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

            var containers = await dbContext.Users
                .AsNoTracking()
                .Include(u => u.Containers)
                .ThenInclude(c => c.Machine)
                .Include(u => u.Containers)
                .ThenInclude(c => c.Image)
                //.Where(u => u.Id == userId)
                .Where(u => u.Id == userId)
                .Select(u => u.Containers)
                .FirstOrDefaultAsync();

            if (containers == null)
                return Ok(new List<ContainerModel>());

            foreach (var container in containers)
            {
                container.IsWindows = container.Machine.IsWindows;
                container.DockerImageName = container.Image.DockerImageName;
                // Sanitize user-shown objects
                container.Machine = null;
                container.Image = null;
            }

            return Ok(containers);
        }

        [HttpGet("{containerId}")]
        public async Task<IActionResult> ContainerDetails(string containerId)
        {
            // Get instance of appropriate host client and handle any fail to get one
            ContainerModel container;
            DockerClient dockerClient;
            try
            {
                // Doesn't map to db entity
                //container = GetContainerById(containerId);
                // Need to have it map to db entity:
                container = await dbContext.Containers
                    .Include(c => c.Machine)
                    .Include(c => c.Image)
                    .Include(c => c.PortBindings)
                    .FirstOrDefaultAsync(c => c.Id == containerId);
                dockerClient = container
                    .Machine
                    .GetDockerClient();
            }
            catch (Exception) { return NotFound(); }

            #region 217, per operator authorization
            string userId = User.FindFirst(x => x.Type.Equals(ClaimTypes.NameIdentifier))?.Value;

            var user = await dbContext.Users
                .AsNoTracking()
                .Include(u => u.Containers)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (!user.Containers.Contains(container))
                return Unauthorized();
            #endregion

            var state = await dockerClient.Containers.InspectContainerAsync(containerId, default);

            if (state != null)
            {
                container.IsRunning = state.State.Running;
                container.State = state.State.Status;
                container.IsWindows = container.Machine.IsWindows;
                container.DockerImageName = container.Image.DockerImageName;
                dbContext.SaveChangesAsync().Wait();
                // Sanitize user-shown objects
                container.Machine = null;
                container.Image = null;
                return Ok(container);
            } else
            {
                dbContext.Entry(container).State = EntityState.Detached;
                container.State = "Unknown (No reply from Docker API)";
                container.IsWindows = container.Machine.IsWindows;
                container.DockerImageName = container.Image.DockerImageName;
                // Sanitize user-shown objects
                container.Machine = null;
                container.Image = null;
                return Ok(dbContext);
            }
        }

        [HttpPost("edit")]
        public async Task<IActionResult> EditContainer(ContainerModel model)
        {
            var container = dbContext.Containers
                .FirstOrDefault(c => c.Id == model.Id);

            if (container != null)
            {
                #region 217, per operator authorization
                string userId = User.FindFirst(x => x.Type.Equals(ClaimTypes.NameIdentifier))?.Value;

                var user = await dbContext.Users
                    .AsNoTracking()
                    .Include(u => u.Containers)
                    .FirstOrDefaultAsync(u => u.Id == userId);

                if (!user.Containers.Contains(container))
                    return Unauthorized();
                #endregion

                // If anything needs to be changed, change here
                container.Description = model.Description;
                container.Name = model.Name;
                dbContext.SaveChanges();
                return Ok();
            }
            return NotFound();
        }

        [HttpPost("start/{containerId}")]
        public async Task<IActionResult> StartContainer(string containerId)
        {
            // Get instance of appropriate host client and handle any fail to get one
            ContainerModel container;
            DockerClient dockerClient;
            try
            {
                container = GetContainerById(containerId);
                dockerClient = container
                    .Machine
                    .GetDockerClient();
            }
            catch (Exception) { return NotFound(); }

            #region 217, per operator authorization
            string userId = User.FindFirst(x => x.Type.Equals(ClaimTypes.NameIdentifier))?.Value;

            var user = await dbContext.Users
                .AsNoTracking()
                .Include(u => u.Containers)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (!user.Containers.Contains(container))
                return Unauthorized();
            #endregion


            var success = await dockerClient.Containers
                .StartContainerAsync(containerId, new ContainerStartParameters());

            if (success)
            {
                var result = await dbContext.Containers.SingleOrDefaultAsync(c => c.Id == containerId);
                if (result != null)
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
            // Get instance of appropriate host client and handle any fail to get one
            ContainerModel container;
            DockerClient dockerClient;
            try
            {
                container = GetContainerById(containerId);
                dockerClient = container
                    .Machine
                    .GetDockerClient();
            }
            catch (Exception) { return NotFound(); }

            #region 217, per operator authorization
            string userId = User.FindFirst(x => x.Type.Equals(ClaimTypes.NameIdentifier))?.Value;

            var user = await dbContext.Users
                .AsNoTracking()
                .Include(u => u.Containers)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (!user.Containers.Contains(container))
                return Unauthorized();
            #endregion

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

        // TODO 216 Zadanie dodatkowe
        [HttpGet("stats/{containerId}")]
        public async Task<IActionResult> GetContainerStats(string containerId)
        {
            // Get instance of appropriate host client and handle any fail to get one
            ContainerModel container;
            DockerClient dockerClient;
            try
            {
                // Doesn't map to db entity
                //container = GetContainerById(containerId);
                // Need to have it map to db entity:
                container = await dbContext.Containers
                    .Include(c => c.Machine)
                    .Include(c => c.Image)
                    .FirstOrDefaultAsync(c => c.Id == containerId);
                dockerClient = container
                    .Machine
                    .GetDockerClient();
            }
            catch (Exception) { return NotFound(); }

            #region 217, per operator authorization
            string userId = User.FindFirst(x => x.Type.Equals(ClaimTypes.NameIdentifier))?.Value;

            var user = await dbContext.Users
                .AsNoTracking()
                .Include(u => u.Containers)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (!user.Containers.Contains(container))
                return Unauthorized();
            #endregion

            var state = await dockerClient.Containers.InspectContainerAsync(containerId, default);

            if(state != null)
            {
                container.IsRunning = state.State.Running;
                container.State = state.State.Status;
                container.DockerImageName = container.Image.DockerImageName;
                dbContext.SaveChangesAsync().Wait();
                return Ok(container);
            }
            return NotFound();

            //throw new NotImplementedException("ProITM.Server.Controllers.ContainerController.GetContainerStats(string containerId)");
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateContainer(ContainerModel model)
        {
            // Get instance of appropriate host client and handle any fail to get one
            var host = await GetLeastBusyHost(model.IsWindows);

            if (host == null || string.IsNullOrEmpty(host.URI))
                return NotFound();

            // Get creating user Id
            string userId = User.FindFirst(x => x.Type.Equals(ClaimTypes.NameIdentifier))?.Value;

            // Construct model
            var image = await dbContext.Images.SingleOrDefaultAsync(i => i.Id == model.DockerImageName);
            dbContext.Attach(image);
            model.Image = image;

            // TODO 222 Check if machine has selected image, if not: pull by name

            dbContext.Attach(host);
            //var port = new ContainerPortModel() { Id = Guid.NewGuid().ToString(), Port = model.PortNo, Host = host };
            model.Machine = host;
            //model.Port = port;
            model.IsRunning = false;

            // Make new instance of DockerClient from URI
            DockerClient dockerClient = host.GetDockerClient();

            // Find the image we're going to use, and if it's missing - download it
            var imageFromDb = await dbContext.Images
                .FirstOrDefaultAsync(i => i.Id == model.DockerImageName);

            if (imageFromDb == null) return NotFound();

            var images = await dockerClient.Images.ListImagesAsync(new ImagesListParameters());
            if (images.FirstOrDefault(i => i.RepoTags.Count == 0 || i.RepoTags[0] == $"{imageFromDb.DockerImageName}:{imageFromDb.Version}") == null)
            {
                dockerClient.Images.CreateImageAsync(
                    new ImagesCreateParameters
                    {
                        FromImage = imageFromDb.DockerImageName,
                        Tag = imageFromDb.Version
                    }, null, new Progress<JSONMessage>()).Wait();

            }

            var exposedPorts = new Dictionary<string, EmptyStruct>();
            var portBindings = new Dictionary<string, IList<PortBinding>>();

            foreach (var portBind in model.PortBindings)
            {
                portBind.Host = host;
                exposedPorts.Add(portBind.PublicPort.ToString() + "/tcp", default(EmptyStruct));
                portBindings.Add(
                    portBind.PublicPort.ToString()  +  "/tcp", new List<PortBinding>{
                        new PortBinding { HostPort = portBind.PrivatePort.ToString() }
                    });
            }

            CreateContainerResponse result = await dockerClient.Containers.CreateContainerAsync(new CreateContainerParameters()
            {
                Image = model.Image.DockerImageName,
                ExposedPorts = exposedPorts,
                HostConfig = new HostConfig()
                {
                    DNS = new[] { "8.8.8.8", "8.8.4.4" },
                    PortBindings = portBindings
                }
                // TODO 195 bind port
            });

            model.Id = result.ID;

            dbContext.Containers.Add(model);

            var user = dbContext.Users.Include(u => u.Containers).FirstOrDefault(u => u.Id == userId);
            dbContext.Attach(user);
            user.Containers.Add(model);

            await dbContext.SaveChangesAsync();

            //Voodo conversion magic
            IList<string> warnings = result.Warnings;
            List<string> listOfWarnings = (List<string>)warnings;
            var serializer = new XmlSerializer(listOfWarnings.GetType());
            var sw = new StringWriter();
            serializer.Serialize(sw, listOfWarnings);
            string res = sw.ToString();
            return Ok(res);
        }

        [HttpDelete("{containerId}")]
        public async Task<IActionResult> DeleteContainer(string containerId)
        {
            // Get instance of appropriate host client and handle any fail to get one
            ContainerModel container;
            DockerClient dockerClient;
            try
            {
                container = GetContainerById(containerId);
                dockerClient = container
                    .Machine
                    .GetDockerClient();
            }
            catch (Exception) { return NotFound(); }

            #region 217, per operator authorization
            string userId = User.FindFirst(x => x.Type.Equals(ClaimTypes.NameIdentifier))?.Value;

            var user = await dbContext.Users
                .AsNoTracking()
                .Include(u => u.Containers)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (!user.Containers.Contains(container))
                return Unauthorized();
            #endregion

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

        [HttpGet("logs/{containerId}/{since}/{tail}")]
        public async Task<IActionResult> GetContainerLogs(string containerId, string since, string tail)
        {
            // Get instance of appropriate host client and handle any fail to get one
            ContainerModel container;
            DockerClient dockerClient;
            try
            {
                container = GetContainerById(containerId);
                dockerClient = container
                    .Machine
                    .GetDockerClient();
            }
            catch (Exception) { return NotFound(); }

            #region 217, per operator authorization
            string userId = User.FindFirst(x => x.Type.Equals(ClaimTypes.NameIdentifier))?.Value;

            var user = await dbContext.Users
                .AsNoTracking()
                .Include(u => u.Containers)
                .FirstOrDefaultAsync(u => u.Id == userId);

            //if (!user.Containers.Exists(c => c.))
            if (!user.Containers.Contains(container))
                return Unauthorized();
            #endregion

            if (dockerClient == null)
                return Ok(new Tuple<string, string>("Container not found", "Container not found"));

            var log_stream = await dockerClient.Containers.GetContainerLogsAsync(containerId, true, new ContainerLogsParameters { ShowStdout = true, ShowStderr = true, Timestamps = true, Tail = "50" }, default);
            var log_tuple = (await log_stream.ReadOutputToEndAsync(default)).ToTuple();

            var stdout_sp =
                Regex
                .Replace(log_tuple.Item1, "^(?:.{1,})([0-9]{4}.{1,})(?:T)([0-9]{2}.{1,})(?:[.]{1}.{1,})(?:Z)", "<B>[$1 $2]\t</B> ", RegexOptions.Multiline)
                .Replace("\n", "<BR />");
            var stderr_sp =
                Regex
                .Replace(log_tuple.Item2, "^(?:.{1,})([0-9]{4}.{1,})(?:T)([0-9]{2}.{1,})(?:[.]{1}.{1,})(?:Z)", "<B>[$1 $2]\t</B> ", RegexOptions.Multiline)
                .Replace("\n", "<BR />");

            return Ok(new Tuple<string, string>(stdout_sp, stderr_sp));
        }

        [HttpGet("refresh")]
        public async Task<IActionResult> RefreshUserContainers()
        {
            string userId = User.FindFirst(x => x.Type.Equals(ClaimTypes.NameIdentifier))?.Value;

            // Get all user containers
            var containers = await dbContext.Users
                //.AsNoTracking()
                .Include(u => u.Containers)
                .ThenInclude(c => c.Machine)
                .Where(u => u.Id == userId)
                .Select(u => u.Containers)
                .FirstOrDefaultAsync();

            var groupedContainers = containers.GroupBy(c => c.Machine.Id).ToList();

            foreach (var contGroup in groupedContainers)
            {
                if(contGroup.Any())
                {
                    var client = contGroup.FirstOrDefault().Machine.GetDockerClient();

                    var dcContainers = await client.Containers.ListContainersAsync(new ContainersListParameters { All = true });

                    // TODO 271 anything below is to be written/rewritten

                    // Iterating over the set containing the other set is suboptimal
                    //foreach(var dcc in dcContainers)
                    foreach(var dbc in contGroup)
                    {
                        var container = dcContainers.FirstOrDefault(c => c.ID == dbc.Id);
                        if (container == null)
                        {
                            dbc.State = "Unknown (No reply from Docker API)";
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
            foreach (var host in hosts)
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
