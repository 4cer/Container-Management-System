using Microsoft.AspNetCore.Identity;
using ProITM.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProITM.Server.Models
{
    public class ApplicationUser : IdentityUser
    {
         public virtual ContainerModel Containers {get; set;}
    }
}
