using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProITM.Shared
{
    public class UserInRole
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public bool IsSelected { get; set; } = false;
    }
}
