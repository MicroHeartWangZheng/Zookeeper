using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace CustomerMicroService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //获取配置
            var config = new ConfigurationBuilder()
                                //需要先设置路径 然后在路径中找到json文件
                                .SetBasePath(Directory.GetCurrentDirectory())
                                .AddJsonFile($"appsettings.json", true, true)
                                .Build();

            //设置地址和端口号
            CreateWebHostBuilder(args)
                .UseUrls("http://127.0.0.1:" + config["Port"])
                .UseConfiguration(config)
                .Build()
                .Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
