using ProITM.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
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

        // TODO 134 Implement ImageService methods

        public async Task<List<ImageModel>> GetImageList()
        {
            return await _httpClient.GetFromJsonAsync<List<ImageModel>>($"Image/images");
        }

        public async Task<ImageModel> GetImageDetails(string imageId)
        {
            return await _httpClient.GetFromJsonAsync<ImageModel>($"Image/images/{imageId}");
        }

        public async Task<HttpResponseMessage> GetImageFromDockerHub(string name, string version, string description)
        {
            return await _httpClient.PostAsJsonAsync<string>($"Image/images/{name}/{version}", description);
        }

        # region Extra-curricular functionality

        public Task<List<ImageModel>> GetUserImageList(string userId)
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
        #endregion
    }
}
