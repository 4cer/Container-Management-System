using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProITM.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        [HttpGet("images")]
        public async Task GetImageList()
        {
            throw new NotImplementedException("ImageController.GetImageList()");
        }

        [HttpGet("images/{id}")]
        public async Task GetImageDetails(string imageId)
        {
            throw new NotImplementedException("ImageController.GetImageDetails()");
        }

        [HttpGet("images/users/{id}")]
        public async Task GetUserImageList(string userId)
        {
            throw new NotImplementedException("ImageController.GetUserImageList()");
        }

        [HttpPost("images/uploadurl")]
        public async Task UploadImageFromUrl()
        {
            throw new NotImplementedException("ImageController.UploadImageFromUdl");
        }

        // Additional Functionality below
        [HttpPost("images/upload/compiled")]
        public async Task UploadCompiledImage()
        {
            throw new NotImplementedException("ImageController.UploadCompiledImage()");
        }

        [HttpPost("images/upload/uncompiled")]
        public async Task UploadUncompiledImage()
        {
            throw new NotImplementedException("ImageController.UploadUncompiledImage()");
        }

        [HttpPost("images/build/{id}")]
        public async Task BuildImage(string imageId)
        {
            throw new NotImplementedException("ImageController.BuildImage()");
        }

        [HttpGet("images/export/{id}")]
        public async Task ExportImage(string userId)
        {
            throw new NotImplementedException("ImageController.ExportImage()");
        }
    }
}
