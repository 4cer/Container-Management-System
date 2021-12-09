﻿using ProITM.Shared;
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

        public async Task<List<ContainerModel>> ListContainers(string userId, long limit)
        {
            return await _httpClient.GetFromJsonAsync<List<ContainerModel>>($"Container/{limit}");
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

        public async Task<HttpResponseMessage> CreateContainer(ContainerModel model)
        {
            return await _httpClient.PostAsJsonAsync<ContainerModel>($"Container/containers/create", model);
        }

        //TODO format since i tail
        public async Task<Stream> GetContainerLogs(string containerId, string since, string tail)
        {
            return await _httpClient.GetFromJsonAsync<Stream>($"Container/containers/logs/{containerId}/{since}/{tail}");
        }
    }
}
