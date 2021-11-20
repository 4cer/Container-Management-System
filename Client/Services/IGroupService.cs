using ProITM.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ProITM.Client.Services
{
    public interface IGroupService
    {
        public Task<List<Group>> GetGroups();
        public Task<Group> GetGroup(string id);
        public Task<HttpResponseMessage> CreateGroup(Group group);
        public Task<HttpResponseMessage> EditGroup(Group group);
        public Task<HttpResponseMessage> DeleteGroup(string id);
        public Task<List<UserInRole>> GetUsersInRole(string id);
        public Task<HttpResponseMessage> EditUsersInRole(List<UserInRole> usersInRole, string id);
        public Task<Group> GetAdminRoleId();
        public Task<List<UserModel>> GetAdmins();
    }
}
