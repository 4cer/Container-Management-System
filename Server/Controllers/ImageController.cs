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
                .FirstAsync(i => i.Id == image.Id);

            if (foundImage == null) return BadRequest();

            foundImage.ImageId = image.ImageId;
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


        [HttpPost("upload")]
        public async Task<IActionResult> GetImageFromDockerHub(ImageModel image)
        {
            image.Created = DateTime.Now;
            dbContext.Images.Add(image);
            await dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{imageId}")]
        public async Task<IActionResult> DeleteImage(string imageId)
        {
            ImageModel model = new() { Id = imageId };
            dbContext.Attach(model);
            dbContext.Remove(model);
            dbContext.SaveChanges();
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
