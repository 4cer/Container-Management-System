using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProITM.Server.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProITM.Shared;
using Docker.DotNet;
using Microsoft.AspNetCore.Authorization;
using ProITM.Server.Utilities;
using Docker.DotNet.Models;
using System.Threading;

namespace ProITM.Server.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;

        public ImageController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpPut("edit")]
        public async Task<IActionResult> EditImage(ImageModel image)
        {
            var foundImage = await dbContext.Images
                .FirstOrDefaultAsync(i => i.Id == image.Id);

            if (foundImage == null) return BadRequest();

            foundImage.DockerImageName = image.DockerImageName;
            foundImage.Description = image.Description;

            if (await dbContext.SaveChangesAsync() == 1)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet("images")]
        public async Task<IActionResult> GetImageList()
        {
            return Ok(dbContext.Images.ToList());
        }

        [HttpGet("{imageId}")]
        public async Task<IActionResult> GetImageDetails(string imageId)
        {
            return Ok(dbContext.Images.Find(imageId));
        }

        // TODO 222 dis needs to be fixed, delegated to helper method
        [Obsolete]
        [HttpPost("upload/{name}/{version}")]
        public async Task<IActionResult> GetImageFromDockerHub(string name, string version, [FromBody] string description)
        {
            ImageModel model = new ImageModel();
            //model.DockerImageName = null;//A to Id nie powinno byc w bazie, bo na kazdej maszynce bedzie inne
            //I dopierow przy klikaniu "uzyj obrazu X" powinno byc sciagane na danego dockera, bo tak bedzie prosciej.
            model.Created = DateTime.Now;
            model.DisplayName = name;
            model.Description = description;
            // 264 - wersja zawsze latest
            model.Version = "latest";
            
            dbContext.Images.Add(model);
            await dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{imageId}")]
        public async Task<IActionResult> DeleteImage(string imageId)
        {
            var image = await dbContext.Images
                // Must track
                //.AsNoTracking()
                .FirstOrDefaultAsync(i => i.Id == imageId);

            if (image == null) return NotFound();

            var containers = await dbContext.Containers
                .AsNoTracking()
                .Include(c => c.Image)
                .Where(c => c.Image.Id == imageId)
                .ToListAsync();

            if (containers.Count > 0) return BadRequest($"Image in use by {containers.Count} containers");

            var hosts = await dbContext.Hosts
                .ToListAsync();

            foreach(var h in hosts)
            {
                // underscore discards warning
                _ = h.GetDockerClient()
                    .Images
                    .DeleteImageAsync($"{image.DockerImageName}:{image.Version}", new ImageDeleteParameters
                    {
                        Force = true
                    });
            }

            dbContext.Images.Remove(image);
            await dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("search/{term}/{official}/{automated}")]
        public async Task<IActionResult> SearchImages(string term, bool official, bool automated)
        {
            DockerClient client;
            try
            {
                var host = dbContext.Hosts.First();
                client = host.GetDockerClient();
            }
            catch (Exception)
            {
                return NotFound();
            }

            var filters = new Dictionary<string, IDictionary<string, bool>>
            {
                //{ "is-official", new Dictionary<string, bool> { { "true", true } } },
                //{ "is-automated", new Dictionary<string, bool> { { "true", true } } }
            };

            if (official)
                filters.Add( "is-official", new Dictionary<string, bool> { { "true", true } } );
            if (automated)
                filters.Add( "is-automated", new Dictionary<string, bool> { { "true", true } } );

            ImagesSearchParameters parameters = new()
            {
                Limit = 10,
                Term = term,
                Filters = filters
            };

            var images = await client.Images.SearchImagesAsync(parameters, default);
            var outImages = new List<ImageHubModel>();

            // Client has no concept of ImageSearchResponse, so gotta translate
            foreach (var image in images)
            {
                outImages.Add(new ImageHubModel()
                {
                    Name = image.Name,
                    Description = image.Description,
                    Is_automated = image.IsAutomated,
                    Is_official = image.IsOfficial,
                    Star_count = image.StarCount
                });
            }

            return Ok(outImages);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateImage(ImageModel model)
        {
            model.Version = "latest";
            await dbContext.Images.AddAsync(model);
            await dbContext.SaveChangesAsync();
            return Ok();
        }

        #region Extra-curricular functionality
        // TODO 186 Implement extracurricular methods
        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetUserImageList(string userId)
        {
            throw new NotImplementedException("ImageController.GetUserImageList()");
        }

        [HttpPost("images/upload/compiled")]
        public async Task<IActionResult> UploadCompiledImage()
        {
            throw new NotImplementedException("ImageController.UploadCompiledImage()");
        }

        [HttpPost("images/upload/uncompiled")]
        public async Task<IActionResult> UploadUncompiledImage()
        {
            throw new NotImplementedException("ImageController.UploadUncompiledImage()");
        }

        [HttpPost("images/build/{id}")]
        public async Task<IActionResult> BuildImage(string imageId)
        {
            throw new NotImplementedException("ImageController.BuildImage()");
        }

        [HttpGet("images/export/{id}")]
        public async Task<IActionResult> ExportImage(string userId)
        {
            throw new NotImplementedException("ImageController.ExportImage()");
        }
        #endregion
    }
}
