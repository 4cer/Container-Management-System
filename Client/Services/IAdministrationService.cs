using ProITM.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ProITM.Client.Services
{
    public interface IAdministrationService
    {
        public Task<List<UserInRole>> GetUsersInRole(string id);
        public Task<HttpResponseMessage> EditUsersInRole(List<UserInRole> usersInRole, string id);
        public Task<Group> GetAdminRoleId();
        public Task<List<UserModel>> GetAdmins();
        public Task<List<UserModel>> GetUsers();
        public Task<UserModel> GetUser(string id);
        public Task<HttpResponseMessage> EditUser(UserModel user);
        public Task<HttpResponseMessage> DeleteUser(string id);
        public Task<HttpResponseMessage> PromoteUser(string id);
        public Task<HttpResponseMessage> DemoteUser(string id);
    }
}
