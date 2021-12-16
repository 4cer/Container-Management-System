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

        public async Task<List<ContainerModel>> GetUserContainers(string userId, int limit)
        {
            return await _httpClient.GetFromJsonAsync<List<ContainerModel>>($"Container/manage/{userId}/{limit}");
        }

        public async Task<List<ContainerModel>> GetContainers(int limit)
        {
            return await _httpClient.GetFromJsonAsync<List<ContainerModel>>($"Container/manage/list/{limit}");
        }
        public async Task<bool> RefreshAllContainers()
        {
            return await _httpClient.GetFromJsonAsync<bool>("Container/manage/refresh");
        }

        public async Task<ContainerModel> ContainerDetails(string containerId)
        {
            return await _httpClient.GetFromJsonAsync<ContainerModel>($"Container/manage/{containerId}");
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
