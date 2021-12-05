using ProITM.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ProITM.Client.Services
{
    interface IAdminContainerService
    {
        public Task<List<ContainerModel>> GetUserContainers(string userid);
        public Task<List<ContainerModel>> GetContainers();
        // TODO may be better to implement as just Task
        public Task<HttpResponseMessage> StartUsersContainer(string containerid);
        public Task<HttpResponseMessage> StopUsersContainer(string containerid);
        public Task<HttpResponseMessage> DeleteUsersContainer(string containerid);
    }
}
