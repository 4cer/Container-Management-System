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
        private List<ContainerModel> Containers = new()
        {
            new ContainerModel() { Id = "0", Name = "name", Image = null, Description = "dupa", Port = null, Machine = null, State = "state1", IsRunning = true },
            new ContainerModel() { Id = "1", Name = "name", Image = null, Description = "dupa", Port = null, Machine = null, State = "state1", IsRunning = true },
            new ContainerModel() { Id = "2", Name = "name", Image = null, Description = "dupa", Port = null, Machine = null, State = "state1", IsRunning = true },
            new ContainerModel() { Id = "3", Name = "name", Image = null, Description = "dupa", Port = null, Machine = null, State = "state1", IsRunning = true },
            new ContainerModel() { Id = "4", Name = "name", Image = null, Description = "dupa", Port = null, Machine = null, State = "state1", IsRunning = true },
            new ContainerModel() { Id = "5", Name = "name", Image = null, Description = "dupa", Port = null, Machine = null, State = "state1", IsRunning = true },
            new ContainerModel() { Id = "6", Name = "name", Image = null, Description = "dupa", Port = null, Machine = null, State = "state1", IsRunning = true },
            new ContainerModel() { Id = "7", Name = "name", Image = null, Description = "dupa", Port = null, Machine = null, State = "state1", IsRunning = true },
            new ContainerModel() { Id = "8", Name = "name", Image = null, Description = "dupa", Port = null, Machine = null, State = "state1", IsRunning = true },
            new ContainerModel() { Id = "8", Name = "name", Image = null, Description = "dupa", Port = null, Machine = null, State = "state1", IsRunning = true }
        };

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
            return Task.FromResult(this.Containers);
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
