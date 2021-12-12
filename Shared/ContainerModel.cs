﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProITM.Shared
{
    public class ContainerModel
    {
        // CANNOT BE AUTOGENERATED, BINDS TO DOCKER
        public string Id { get; set; }

        public string Name { get; set; }

        public ImageModel Image { get; set; }

        public string Description { get; set; }

        public ContainerPortModel Port { get; set; }

        public HostModel Machine { get; set; }

        // Ignore for DB purposes
        public string State { get; set; }

        public bool IsRunning { get; set; }

        // TODO 148 Describe additional Container datum

        [NotMapped] // For creation purposes
        public bool IsWindows { get; set; }

        // Numeric port number passed by the form
        [NotMapped]
        public int PortNo { get; set; }

        // Docker Image name passed by container form at creation
        [NotMapped]
        public string ImageIdC { get; set; }

        // Username of container owner, for display purposes
        [NotMapped]
        public string OwnerName { get; set; }
    }
}
