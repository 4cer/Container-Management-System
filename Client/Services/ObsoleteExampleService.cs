using ProITM.Shared;
using System.Collections.Generic;
// Two below are important
using System.Net.Http;
// This just for ease of use
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ProITM.Client.Services
{
    public class ObsoleteExampleService : IObsoleteExampleService
    {
        private readonly HttpClient _httpClient;

        // Dependency insertion for an object handling http requests
        public ObsoleteExampleService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // ======================
        //        OBSOLETE
        //  But left as examples
        // ======================

        public async Task<List<Group>> GetGroups()
        {
            // GET method return can be anything as long as it is usable
            // on client and consistent with expected type
            // If you expect no return value, don't use a GET mehod,
            return await _httpClient.GetFromJsonAsync<List<Group>>("ObsoleteExample");
        }

        public async Task<Group> GetGroup(string id)
        {
            // GET method return can be anything as long as it is usable
            // on client and consistent with expected type
            // If you expect no return value, don't use a GET mehod,
            return await _httpClient.GetFromJsonAsync<Group>($"ObsoleteExample/{id}");
        }

        public async Task<HttpResponseMessage> CreateGroup(Group group)
        {
            // Return on a POST can be whatever, as long as it is usable
            // on client and consistent with expected type
            // if not important, just plug HttpResponseMessage
            // which can help in debugging
            //var result = await _httpClient.PostAsJsonAsync<Group>("Group", group);
            return await _httpClient.PostAsJsonAsync<Group>("ObsoleteExample", group);
        }

        public async Task<HttpResponseMessage> EditGroup(Group group)
        {
            // Canonically POST is used for creating new resources
            // Updating, being a replacement can be a PUT method
            // doesn't really matter that much. Use if differentiation
            // on same route is necessary
            // like POST:/user - create, PUT:/user - update etc
            return await _httpClient.PostAsJsonAsync<Group>("ObsoleteExample/Edit", group);
        }

        public async Task<HttpResponseMessage> DeleteGroup(string id)
        {
            // Cannonically deletion should be done using DELETE method
            // can be POST, but never GET, as it becomes a vulnerability
            // to malicious link attacks, like a mail with GET:.../delete/{id} route
            // using your browser, with whatever authentication state you possess
            return await _httpClient.DeleteAsync($"ObsoleteExample/delete/{id}");
            //return await _httpClient.PostAsJsonAsync<Group>("Group/Delete", group);
        }
    }
}
