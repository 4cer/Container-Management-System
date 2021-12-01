using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProITM.Shared
{
    public class HostModel
    {
        public string Id { get; set; }

        public string DisplayName { get; set; }

        public bool IsWindows { get; set; }

        public string IP { get; set; }

        public int Port { get; set; }

        public string URI { get; set; }

        // TODO 149 Describe Host datum
    }
}
