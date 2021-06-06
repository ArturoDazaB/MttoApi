using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

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