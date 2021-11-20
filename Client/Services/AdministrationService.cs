using ProITM.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ProITM.Client.Services
{
    public class AdministrationService : IAdministrationService
    {
        private readonly HttpClient _httpClient;

        public AdministrationService(HttpClient httpClient)
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

        // =======================
        // TODO Above are obsolete
        // =======================

        public async Task<List<UserInRole>> GetUsersInRole(string id)
        {
            return await _httpClient.GetFromJsonAsync<List<UserInRole>>($"Administration/Edit/{id}");
        }

        public async Task<HttpResponseMessage> EditUsersInRole(List<UserInRole> usersInRole, string id)
        {
            //Console.WriteLine("GroupService.EditUsersInRole: " + id);
            return await _httpClient.PostAsJsonAsync<List<UserInRole>>($"Administration/Edit/{id}", usersInRole);
        }

        public async Task<Group> GetAdminRoleId()
        {
            return await _httpClient.GetFromJsonAsync<Group>("Administration/adminroleid");
        }

        public async Task<List<UserModel>> GetAdmins()
        {
            return await _httpClient.GetFromJsonAsync<List<UserModel>>("Administration/adminroleusers");
        }

        public async Task<List<UserModel>> GetUsers()
        {
            return await _httpClient.GetFromJsonAsync<List<UserModel>>("Administration/users");
        }

        public async Task<List<UserModel>> GetUser(string id)
        {
            return await _httpClient.GetFromJsonAsync<List<UserModel>>($"Administration/users/{id}");
        }

        public async Task<HttpResponseMessage> DeleteUser(string id)
        {
            return await _httpClient.DeleteAsync($"Administration/users/{id}");
        }

        public async Task<HttpResponseMessage> PromoteUser(string id)
        {
            return await _httpClient.PostAsJsonAsync<string>($"Administration/users/promote/{id}", null);
        }

        public async Task<HttpResponseMessage> DemoteUser(string id)
        {

            return await _httpClient.PostAsJsonAsync<string>($"Administration/users/demote/{id}", null);
        }
    }
}
