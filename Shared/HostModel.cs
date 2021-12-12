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
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        public string DisplayName { get; set; }

        public bool IsWindows { get; set; }

        // Unused, as expected
        public string IP { get; set; }

        // Unused, as expected
        public int Port { get; set; }

        // Only addressing field in actual use
        public string URI { get; set; }

        // TODO 149 Describe Host datum
    }
}
