using ProITM.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ProITM.Client.Services
{
    interface IImageService
    {
        public Task<List<ImageModel>> GetImageList();

        public Task<ImageModel> GetImageDetails(string imageId);

        public Task<List<ImageModel>> GetUserImageList(string userId);

        public Task<HttpResponseMessage> GetImageFromDockerHub(ImageModel image);

        public Task<HttpResponseMessage> DeleteImage(string imageId);

        public Task<HttpResponseMessage> UploadCompiledImage();

        public Task<HttpResponseMessage> UploadUncompiledImage();

        public Task<HttpResponseMessage> BuildImage(string imageId);

        public Task<HttpResponseMessage> ExportImage(string userId);
    }
}
