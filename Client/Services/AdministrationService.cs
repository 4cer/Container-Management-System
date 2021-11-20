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

        public async Task<List<UserModel>> GetUsers()
        {
            return await _httpClient.GetFromJsonAsync<List<UserModel>>("Administration/users");
        }

        public async Task<UserModel> GetUser(string id)
        {
            return await _httpClient.GetFromJsonAsync<UserModel>($"Administration/users/{id}");
        }

        public async Task<HttpResponseMessage> EditUser(UserModel user)
        {
            return await _httpClient.PostAsJsonAsync<UserModel>($"Administration/users/Edit", user);
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

        public async Task<Group> GetAdminRoleId()
        {
            return await _httpClient.GetFromJsonAsync<Group>("Administration/Adminroleid");
        }

        public async Task<List<UserModel>> GetAdmins()
        {
            return await _httpClient.GetFromJsonAsync<List<UserModel>>("Administration/Adminroleusers");
        }

        public async Task<List<UserInRole>> GetUsersInRole(string id)
        {
            return await _httpClient.GetFromJsonAsync<List<UserInRole>>($"Administration/Edit/{id}");
        }

        public async Task<HttpResponseMessage> EditUsersInRole(List<UserInRole> usersInRole, string id)
        {
            return await _httpClient.PostAsJsonAsync<List<UserInRole>>($"Administration/Edit/{id}", usersInRole);
        }
    }
}
