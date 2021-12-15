using Docker.DotNet;
using ProITM.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProITM.Server.Utilities
{
    public static class HostModelUtils
    {
        public static DockerClient GetDockerClient(this HostModel host)
        {
            string address;
            if (host.URI != null && host.URI.Length > 0)
            {
                address = host.URI;
            }
            else
            {
                address = $"{host.IP}:{host.Port}";
            }

            return new DockerClientConfiguration(new Uri(address))
                .CreateClient();
        }

        public static DockerClient GetDockerClient(string hostUri)
        {
            return new DockerClientConfiguration(new Uri(hostUri))
                .CreateClient();
        }
    }
}
