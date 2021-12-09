using ProITM.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ProITM.Client.Services
{
    public class HostService : IHostService
    {
        private readonly HttpClient _httpClient;

        public HostService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<HttpResponseMessage> AddHost(HostModel host)
        {
            return await _httpClient.PostAsJsonAsync<HostModel>($"Host/create", host);
        }

        public async Task<HttpResponseMessage> DeleteHost(string hostId)
        {
            return await _httpClient.DeleteAsync($"Host/{hostId}");
        }

        public async Task<List<HostModel>> GetHosts()
        {
            return await _httpClient.GetFromJsonAsync<List<HostModel>>("Host/list");
        }

        public async Task<HostModel> HostDetails(string hostId)
        {
            return await _httpClient.GetFromJsonAsync<HostModel>($"Host/{hostId}");
        }

        public async Task<HttpResponseMessage> GetHostLogs(string hostId)
        {
            return await _httpClient.GetAsync($"Host/logs/{hostId}");
        }
    }
}
