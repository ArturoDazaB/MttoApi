using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MttoApi
{
    //=========================================================================================================
    //=========================================================================================================
    //ASP.NET CORE APPLICATION MUST INCLUDE A "STAR UP" CLASS. IT IS EXECUTED FIRST WHEN THE APPLICATION 
    //STARTS. THIS CLASS CAN BE CONFIGURED USING.
    //=========================================================================================================
    //=========================================================================================================
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
                });
    }
}
