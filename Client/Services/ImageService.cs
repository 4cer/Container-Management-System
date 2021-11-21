using ProITM.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ProITM.Client.Services
{
    public class ImageService : IImageService
    {
        private readonly HttpClient _httpClient;

        public ImageService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // TODO Implement ImageService methods

        public Task<List<ImageModel>> GetImageList()
        {
            throw new NotImplementedException();
        }

        public Task<ImageModel> GetImageDetails(string imageId)
        {
            throw new NotImplementedException();
        }

        public Task<List<ImageModel>> GetUserImageList(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<HttpResponseMessage> UploadImageFromUrl()
        {
            throw new NotImplementedException();
        }

        public Task<HttpResponseMessage> UploadCompiledImage()
        {
            throw new NotImplementedException();
        }

        public Task<HttpResponseMessage> UploadUncompiledImage()
        {
            throw new NotImplementedException();
        }

        public Task<HttpResponseMessage> BuildImage(string imageId)
        {
            throw new NotImplementedException();
        }

        public Task<HttpResponseMessage> ExportImage(string userId)
        {
            throw new NotImplementedException();
        }
    }
}
