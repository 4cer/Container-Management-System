using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace ProITM.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();

                    //webBuilder.ConfigureKestrel(o =>
                    //{
                    //    o.ConfigureHttpsDefaults(o =>
                    //        o.ClientCertificateMode =
                    //        ClientCertificateMode.RequireCertificate);
                    //});
                });
    }
}
