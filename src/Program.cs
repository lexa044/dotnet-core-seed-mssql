using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

using log4net;
using log4net.Config;

namespace DNSeed
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //DateTime utcNow = DateTime.Now;//DateTime.UtcNow;
            //DateTimeOffset jsOffset= DateTimeOffset.FromUnixTimeMilliseconds(1616434269705L);
            //Console.WriteLine("********************************");
            //Console.WriteLine(jsOffset);
            //Console.WriteLine(jsOffset.DateTime);
            //Console.WriteLine(jsOffset.DateTime.ToLocalTime());
            //DateTimeOffset utcNowOffset = new DateTimeOffset(utcNow.ToUniversalTime());
            //Console.WriteLine("********************************");
            //Console.WriteLine(utcNowOffset.ToUnixTimeMilliseconds());
            //Console.WriteLine(utcNowOffset.ToUnixTimeSeconds());
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));

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
