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
        public Task<ContainerModel> ContainerDetails(string containerId);
        // TODO may be better to implement as just Task
        public Task<HttpResponseMessage> StartContainer(string containerid);
        public Task<HttpResponseMessage> StopContainer(string containerid);
        public Task<System.IO.Stream> GetContainerStats(string containerid);
        public Task<List<string>> CreateContainer(ContainerModel model);
        public Task<HttpResponseMessage> DeleteContainer(string containerid);
        public Task<Tuple<string, string>> GetContainerLogs(string containerid, string since, string tail);
    }
}
