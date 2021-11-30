using ProITM.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ProITM.Client.Services
{
    public class ContainerService : IContainerService
    {
        public Task<HttpResponseMessage> CreateContainer()
        {
            throw new NotImplementedException("ContainerService.CreateContainer()");
        }

        public Task<HttpResponseMessage> DeleteContainer(string containerId)
        {
            throw new NotImplementedException("ContainerService.DeleteContainer(string containerId)");
        }

        public Task<List<string>> GetContainerLogs(string containerId)
        {
            throw new NotImplementedException("ContainerService.GetContainerLogs(string containerId)");
        }

        public Task<ContainerModel> GetContainerStats(string containerId)
        {
            throw new NotImplementedException("ContainerService.GetContainerStats(string containerId)");
        }

        public Task<List<ContainerModel>> ListContainers(string userId)
        {
            throw new NotImplementedException("ContainerService.ListContainers(string userId)");
        }

        public Task<HttpResponseMessage> StartContainer(string containerId)
        {
            throw new NotImplementedException("ContainerService.StartContainer(string containerId)");
        }

        public Task<HttpResponseMessage> StopContainer(string containerId)
        {
            throw new NotImplementedException("ContainerService.StopContainer(string containerId)");
        }
    }
}
