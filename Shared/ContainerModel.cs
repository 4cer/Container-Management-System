using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProITM.Shared
{
    public class ContainerModel
    {
        public string Id { get; set; }

        // TODO Pull UserModel to get access to class:
        //public UserModel Owner { get; set; }

        public string Name { get; set; }

        public string ImageId { get; set; }

        public string Description { get; set; }

        public int PortId { get; set; }

        public HostModel Machine { get; set; }

        // Ignore for DB purposes
        public string State { get; set; }

        public bool IsRunning { get; set; }

        // TODO 148 Describe additional Container datum
    }
}
