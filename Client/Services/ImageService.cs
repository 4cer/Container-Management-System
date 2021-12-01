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
        private List<ImageModel> Images = new()
        {
            new ImageModel() { Id = "0", ImageId = "name", Created = DateTime.Now, Name = "Name", Description = "Dupa", Version = "1.0" },
            new ImageModel() { Id = "1", ImageId = "name", Created = DateTime.Now, Name = "Name", Description = "Dupa", Version = "1.0" },
            new ImageModel() { Id = "2", ImageId = "name", Created = DateTime.Now, Name = "Name", Description = "Dupa", Version = "1.0" },
            new ImageModel() { Id = "3", ImageId = "name", Created = DateTime.Now, Name = "Name", Description = "Dupa", Version = "1.0" },
            new ImageModel() { Id = "4", ImageId = "name", Created = DateTime.Now, Name = "Name", Description = "Dupa", Version = "1.0" },
            new ImageModel() { Id = "5", ImageId = "name", Created = DateTime.Now, Name = "Name", Description = "Dupa", Version = "1.0" },
            new ImageModel() { Id = "6", ImageId = "name", Created = DateTime.Now, Name = "Name", Description = "Dupa", Version = "1.0" },
            new ImageModel() { Id = "7", ImageId = "name", Created = DateTime.Now, Name = "Name", Description = "Dupa", Version = "1.0" },
            new ImageModel() { Id = "8", ImageId = "name", Created = DateTime.Now, Name = "Name", Description = "Dupa", Version = "1.0" },
            new ImageModel() { Id = "9", ImageId = "name", Created = DateTime.Now, Name = "Name", Description = "Dupa", Version = "1.0" }
        };

        private readonly HttpClient _httpClient;

        public ImageService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // TODO 134 Implement ImageService methods

        public Task<List<ImageModel>> GetImageList()
        {
            return Task.FromResult(this.Images);
        }

        public Task<ImageModel> GetImageDetails(string imageId)
        {
            return Task.FromResult(Images.Find(img => img.Id == imageId));
        }

        

        public Task<HttpResponseMessage> GetImageFromDockerHub()
        {
            throw new NotImplementedException();
        }

        // Extra-curricular functionality below

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
    }
}
