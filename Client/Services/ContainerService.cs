using ProITM.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ProITM.Client.Services
{
    public class ContainerService : IContainerService
    {
        private readonly HttpClient _httpClient;

        public ContainerService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ContainerModel>> GetUserContainers(string userId)
        {
            return await _httpClient.GetFromJsonAsync<List<ContainerModel>>($"Container/{userId}");
        }

        public async Task<HttpResponseMessage> StartUsersContainer(string containerId)
        {
            return await _httpClient.PostAsJsonAsync<string>($"Container/start/{containerId}", null);
        }

        public async Task<HttpResponseMessage> StopUsersContainer(string containerId)
        {
            return await _httpClient.PostAsJsonAsync<string>($"Container/stop/{containerId}", null);
        }

        public async Task<HttpResponseMessage> DeleteUsersContainer(string containerId)
        {
            return await _httpClient.DeleteAsync($"Container/{containerId}");
        }
    }
}
