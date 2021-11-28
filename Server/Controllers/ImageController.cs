using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProITM.Server.Data;

namespace ProITM.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        // TODO 156 Inject database ApplicationDbContext

        // TODO 147 implement ImageController endpoint methods
        private readonly ApplicationDbContext db;

        public ImageController(ApplicationDbContext _db)
        {
            db = _db;
            db.Database.EnsureCreated();
        }


        [HttpGet("images")]
        public async Task<IActionResult> GetImageList()
        {
            return db.Images.ToList();
            //throw new NotImplementedException("ImageController.GetImageList()");
        }

        [HttpGet("images/{id}")]
        public async Task<IActionResult> GetImageDetails(string imageId)
        {
            return db.Images.Find(imageId);
            //throw new NotImplementedException("ImageController.GetImageDetails()");
        }


        [HttpPost("images/upload/{url}")]
        public async Task<IActionResult> UploadImageFromUrl(string url)
        {
            ImageModel model = new ImageModel();
            model.Id = null;//Tu trzeba to przegadać, bo tu cos niejasne. Imo. tu powinnismy sami generowac jakies Id
            model.ImageId = null;//A to Id nie powinno byc w bazie, bo na kazdej maszynce bedzie inne
            //I dopierow przy klikaniu "uzyj obrazu X" powinno byc sciagane na danego dockera, bo tak bedzie prosciej.
            model.Created = DateTime.Now;
            model.Name = url;
            model.Description = "Do uzupelnienia";//To nie wiem, ja jestem naprawde chujowy z weba :C, ktos powiedz mi jak dwa argumenty z url zdjac
            model.Tag = "latest"; //do sprawdzenia, czy to sie jakos inaczej da.
            
            db.Images.Add(model);
            db.SaveChangesAsync();
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
