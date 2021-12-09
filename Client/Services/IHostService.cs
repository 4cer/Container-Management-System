using ProITM.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ProITM.Client.Services
{
    interface IHostService
    {
        public Task<HttpResponseMessage> AddHost(HostModel host);
        public Task<HttpResponseMessage> DeleteHost(string hostId);
        public Task<List<HostModel>> GetHosts();
        public Task<HostModel> HostDetails(string hostId);
        public Task<HttpResponseMessage> GetHostLogs(string hostId);
    }
}
