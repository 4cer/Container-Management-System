using ProITM.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ProITM.Client.Services
{
    public class AdminContainerService : IAdminContainerService
    {
        private readonly HttpClient _httpClient;

        public AdminContainerService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ContainerModel>> GetUserContainers(string userId)
        {
            return await _httpClient.GetFromJsonAsync<List<ContainerModel>>($"Container/manage/{userId}");
        }

        public async Task<List<ContainerModel>> GetContainers()
        {
            return await _httpClient.GetFromJsonAsync<List<ContainerModel>>($"Container/manage/list");
        }

        public async Task<HttpResponseMessage> StartUsersContainer(string containerId)
        {
            return await _httpClient.PostAsJsonAsync<string>($"Container/manage/start/{containerId}", null);
        }

        public async Task<HttpResponseMessage> StopUsersContainer(string containerId)
        {
            return await _httpClient.PostAsJsonAsync<string>($"Container/manage/stop/{containerId}", null);
        }

        public async Task<HttpResponseMessage> DeleteUsersContainer(string containerId)
        {
            return await _httpClient.DeleteAsync($"Container/manage/{containerId}");
        }
    }
}
