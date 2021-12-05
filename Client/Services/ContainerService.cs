using ProITM.Shared;
using System;
using System.Collections.Generic;
using System.IO;
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

        //TODO POPRAWIĆ JAK BĘDZIE WŁAŚCIWE ZAPYTANIE W KOTNROLERZE
        public async Task<List<ContainerModel>> ListContainers(long limit)
        {
            return await _httpClient.GetFromJsonAsync<List<ContainerModel>>($"Container//{limit}");
        }

        public async Task<HttpResponseMessage> StartContainer(string containerId)
        {
            return await _httpClient.PostAsJsonAsync<string>($"Container/start/{containerId}", null);
        }

        public async Task<HttpResponseMessage> StopContainer(string containerId)
        {
            return await _httpClient.PostAsJsonAsync<string>($"Container/stop/{containerId}", null);
        }

        public async Task<HttpResponseMessage> DeleteContainer(string containerId)
        {
            return await _httpClient.DeleteAsync($"Container/{containerId}");
        }

        public async Task<Stream> GetContainerStats(string containerId)
        {
            return await _httpClient.GetFromJsonAsync<Stream>($"Container/containers/stats/{containerId}");
        }

        //To na pewno zadziała? żadnej nazwy ani nic
        public async Task<HttpResponseMessage> CreateContainer()
        {
            return await _httpClient.PostAsJsonAsync<string>($"Container/containers/create", null);
        }

        //jak przesłać resztę argumentów?
        public async Task<Stream> GetContainerLogs(string containerId, string since, string tail)
        {
            return await _httpClient.GetFromJsonAsync<Stream>($"Container/containers/logs/{containerId}");
        }
    }
}
