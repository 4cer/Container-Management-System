using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProITM.Server.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProITM.Server.Data;
using ProITM.Shared;

namespace ProITM.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
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
            return Ok(dbContext.Images.ToList());
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


        // Extra-curricular functionality below

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
    }
}
