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
        public string Id { get; set; }

        public int Port { get; set; }

        public HostModel Host { get; set; }
    }
}