using ProITM.Shared;
using System.Collections.Generic;
// Just to know what plug type HttpResponse... is
using System.Net.Http;
using System.Threading.Tasks;

namespace ProITM.Client.Services
{
    interface IExampleService
    {
        // ======================
        //        OBSOLETE
        //  But left as examples
        // ======================

        public Task<List<Group>> GetGroups();
        public Task<Group> GetGroup(string id);
        public Task<HttpResponseMessage> CreateGroup(Group group);
        public Task<HttpResponseMessage> EditGroup(Group group);
        public Task<HttpResponseMessage> DeleteGroup(string id);
    }
}
