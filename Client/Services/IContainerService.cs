using ProITM.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ProITM.Client.Services
{
    interface IContainerService
    {
        public Task<List<ContainerModel>> ListContainers(string userId);
        public Task<HttpResponseMessage> StartContainer(string containerId);
        public Task<HttpResponseMessage> StopContainer(string containerId);
        public Task<ContainerModel> GetContainerStats(string containerId);
        public Task<HttpResponseMessage> CreateContainer();
        public Task<HttpResponseMessage> DeleteContainer(string containerId);
        public Task<List<string>> GetContainerLogs(string containerId);
        
    }
}
