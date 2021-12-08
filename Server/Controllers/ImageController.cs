using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private List<ImageModel> Images = new()
        {
            new ImageModel() { Id = "0", ImageId = "name", Created = DateTime.Now, Name = "Name", Description = "Dupa", Version = "1.0" },
            new ImageModel() { Id = "1", ImageId = "name", Created = DateTime.Now, Name = "Name", Description = "Dupa", Version = "1.1" },
            new ImageModel() { Id = "2", ImageId = "name", Created = DateTime.Now, Name = "Name", Description = "Dupa", Version = "1.2" },
            new ImageModel() { Id = "3", ImageId = "name", Created = DateTime.Now, Name = "Name", Description = "Dupa", Version = "1.3" },
            new ImageModel() { Id = "4", ImageId = "name", Created = DateTime.Now, Name = "Name", Description = "Dupa", Version = "2.0" },
            new ImageModel() { Id = "5", ImageId = "name", Created = DateTime.Now, Name = "Name", Description = "Dupa", Version = "2.1" },
            new ImageModel() { Id = "6", ImageId = "name", Created = DateTime.Now, Name = "Name", Description = "Dupa", Version = "2.2" },
            new ImageModel() { Id = "7", ImageId = "name", Created = DateTime.Now, Name = "Name", Description = "Dupa", Version = "2.3" },
            new ImageModel() { Id = "8", ImageId = "name", Created = DateTime.Now, Name = "Name", Description = "Dupa", Version = "2.1" },
            new ImageModel() { Id = "9", ImageId = "name", Created = DateTime.Now, Name = "Name", Description = "Dupa", Version = "3.7" }
        };

        private readonly ApplicationDbContext dbContext;

        public ImageController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
            // Do not think below code necessary - Szymon Brawański
            //this.dbContext.Database.EnsureCreated();
        }

        // TODO 147 implement ImageController endpoint methods

        [HttpGet("images")]
        public async Task<IActionResult> GetImageList()
        {
            //return Ok(dbContext.Images.ToList());
            return Ok(Images);
            //throw new NotImplementedException("ImageController.GetImageList()");
        }

        [HttpGet("images/{id}")]
        public async Task<IActionResult> GetImageDetails(string id)
        {
            return Ok(dbContext.Images.Find(id));
            //throw new NotImplementedException("ImageController.GetImageDetails()");
        }


        [HttpPost("images/upload/{name}/{version}")]
        public async Task<IActionResult> GetImageFromDockerHub(string name, string version, [FromBody] string description)
        {
            ImageModel model = new ImageModel();
            model.ImageId = null;//A to Id nie powinno byc w bazie, bo na kazdej maszynce bedzie inne
            //I dopierow przy klikaniu "uzyj obrazu X" powinno byc sciagane na danego dockera, bo tak bedzie prosciej.
            model.Created = DateTime.Now;
            model.Name = name;
            model.Description = description;
            model.Version = version;
            
            dbContext.Images.Add(model);
            await dbContext.SaveChangesAsync();
            return Ok();
            //throw new NotImplementedException("ImageController.UploadImageFromUdl");
        }


        #region Extra-curricular functionality
        [HttpGet("images/users/{id}")]
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
