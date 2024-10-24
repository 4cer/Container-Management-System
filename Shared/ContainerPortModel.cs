﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProITM.Shared
{
    public class ContainerPortModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        [Required]
        public ushort PrivatePort { get; set; }

        public ushort PublicPort { get; set; }

        public HostModel Host { get; set; }

        public override bool Equals(object obj)
        {
            ContainerPortModel model = (ContainerPortModel)obj;
            return model.PrivatePort == this.PrivatePort && model.PublicPort == this.PublicPort;
        }
        public override int GetHashCode()
        {
            return PrivatePort.GetHashCode()^PublicPort.GetHashCode();
        }
    }
}
