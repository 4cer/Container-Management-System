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
        public Task<List<ContainerModel>> ListContainers(long limit);
        // TODO may be better to implement as just Task
        public Task<HttpResponseMessage> StartContainer(string containerid);
        public Task<HttpResponseMessage> StopContainer(string containerid);
        public Task<System.IO.Stream> GetContainerStats(string containerid);
        public Task<HttpResponseMessage> CreateContainer(ContainerModel model);
        public Task<HttpResponseMessage> DeleteContainer(string containerid);
        public Task<System.IO.Stream> GetContainerLogs(string containerid, string since, string tail);
    }
}
