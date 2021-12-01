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
        private List<HostModel> Hosts = new()
        {
            new HostModel() { Id = "0", DisplayName = "name", IsWindows = true, IP = "1.1.1.1", Port = 6969, URI = "www.dupa.com" },
            new HostModel() { Id = "1", DisplayName = "name", IsWindows = true, IP = "1.1.1.1", Port = 6969, URI = "www.dupa.com" },
            new HostModel() { Id = "2", DisplayName = "name", IsWindows = true, IP = "1.1.1.1", Port = 6969, URI = "www.dupa.com" },
            new HostModel() { Id = "3", DisplayName = "name", IsWindows = true, IP = "1.1.1.1", Port = 6969, URI = "www.dupa.com" },
            new HostModel() { Id = "4", DisplayName = "name", IsWindows = true, IP = "1.1.1.1", Port = 6969, URI = "www.dupa.com" },
            new HostModel() { Id = "5", DisplayName = "name", IsWindows = true, IP = "1.1.1.1", Port = 6969, URI = "www.dupa.com" },
            new HostModel() { Id = "6", DisplayName = "name", IsWindows = true, IP = "1.1.1.1", Port = 6969, URI = "www.dupa.com" },
            new HostModel() { Id = "7", DisplayName = "name", IsWindows = true, IP = "1.1.1.1", Port = 6969, URI = "www.dupa.com" },
            new HostModel() { Id = "8", DisplayName = "name", IsWindows = true, IP = "1.1.1.1", Port = 6969, URI = "www.dupa.com" },
            new HostModel() { Id = "9", DisplayName = "name", IsWindows = true, IP = "1.1.1.1", Port = 6969, URI = "www.dupa.com" }
        };

        private readonly HttpClient _httpClient;

        public HostService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<HttpResponseMessage> AddHost(HostModel host)
        {
            return await _httpClient.PostAsJsonAsync<HostModel>($"Host", host);
        }

        public async Task<HttpResponseMessage> DeleteHost(string hostId)
        {
            return await _httpClient.DeleteAsync($"Host/{hostId}");
        }

        public async Task<List<HostModel>> GetHosts()
        {
            return this.Hosts;
        }

        public async Task<HttpResponseMessage> GetHostLogs(string hostId)
        {
            return await _httpClient.GetAsync($"Host/logs/{hostId}");
        }
    }
}
