using ProITM.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ProITM.Client.Services
{
    public class GroupService : IGroupService
    {
        private readonly HttpClient _httpClient;

        public GroupService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Group>> GetGroups()
        {
            return await _httpClient.GetFromJsonAsync<List<Group>>("Group");
        }

        public async Task<Group> GetGroup(string id)
        {
            return await _httpClient.GetFromJsonAsync<Group>($"Group/{id}");
        }

        public async Task<HttpResponseMessage> CreateGroup(Group group)
        {
            //var result = await _httpClient.PostAsJsonAsync<Group>("Group", group);
            return await _httpClient.PostAsJsonAsync<Group>("Group", group);
        }

        public async Task<HttpResponseMessage> EditGroup(Group group)
        {
            return await _httpClient.PostAsJsonAsync<Group>("Group/Edit", group);
        }

        public async Task<HttpResponseMessage> DeleteGroup(string id)
        {
            return await _httpClient.DeleteAsync($"Group/delete/{id}");
            //return await _httpClient.PostAsJsonAsync<Group>("Group/Delete", group);
        }

        public async Task<List<UserInRole>> GetUsersInRole(string id)
        {
            return await _httpClient.GetFromJsonAsync<List<UserInRole>>($"Group/Edit/{id}");
        }

        public async Task<HttpResponseMessage> EditUsersInRole(List<UserInRole> usersInRole, string id)
        {
            //Console.WriteLine("GroupService.EditUsersInRole: " + id);
            return await _httpClient.PostAsJsonAsync<List<UserInRole>>($"Group/Edit/{id}", usersInRole);
        }
    }
}
