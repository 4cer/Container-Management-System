using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProITM.Shared
{
    public class ImageHubModel
    {
        public long Star_count { get; set; }

        public bool Is_official { get; set; }

        // This corresponds to the name images are pulled by
        public string Name { get; set; }

        public bool Is_automated { get; set; }

        public string Description { get; set; }
    }
}
