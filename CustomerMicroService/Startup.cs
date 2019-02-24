using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using org.apache.zookeeper;
using Watch;
using static org.apache.zookeeper.ZooDefs;

namespace CustomerMicroService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            InitZooKeeper();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }


        public void InitZooKeeper()
        {
            var MyApp = "/MyApp";
            //创建ZooKeeper 118.24.96.212
            ZooKeeper zooKeeper = new ZooKeeper("127.0.0.1:2181", 50000, new MyWatcher());

            //创建 MyApp节点，数据为:MyAppData 权限控制为：开放  节点类型为：持久性节点
            if (zooKeeper.existsAsync(MyApp) != null)
                zooKeeper.createAsync(MyApp, Encoding.UTF8.GetBytes("MyAppData"), Ids.OPEN_ACL_UNSAFE, CreateMode.PERSISTENT);

            //通过反射获取所有接口列表
            Dictionary<string, List<string>> serviceAndApiPaths = new Dictionary<string, List<string>>();
            var types = System.Reflection.Assembly.GetExecutingAssembly().GetTypes();
            foreach (var type in types)
            {
                if (type.BaseType == typeof(ControllerBase))
                {
                    var methods = type.GetMethods();
                    foreach (var method in methods)
                    {
                        foreach (var customAttribute in method.CustomAttributes)
                        {
                            if (customAttribute.AttributeType == typeof(RouteAttribute))
                            {
                                var serviceName = type.Name.Replace("Controller", "Services");
                                if (!serviceAndApiPaths.Keys.Contains(serviceName))
                                {
                                    List<string> apiPaths = new List<string>();
                                    apiPaths.Add(customAttribute.ConstructorArguments[0].ToString().Replace("/", "-"));
                                    serviceAndApiPaths.Add(serviceName, apiPaths);
                                }
                                else
                                    serviceAndApiPaths[serviceName].Add(customAttribute.ConstructorArguments[0].ToString().Replace("/", "-"));
                            }
                        }
                    }
                }
            }

            //将这些接口列表 放到MyApp节点下 

            foreach (var item in serviceAndApiPaths)
            {
                //创建 服务节点，为持久性节点
                if (zooKeeper.existsAsync($@"{MyApp}/{item.Key}") != null)
                    zooKeeper.createAsync($@"{MyApp}/{item.Key}", null, Ids.OPEN_ACL_UNSAFE, CreateMode.PERSISTENT);
                foreach (var apiPath in item.Value)
                {
                    //创建 Api节点，为持久性节点
                    if (zooKeeper.existsAsync($@"{MyApp}/{item.Key}/{apiPath}") != null)
                        zooKeeper.createAsync($@"{MyApp}/{item.Key}/{apiPath}", null, Ids.OPEN_ACL_UNSAFE, CreateMode.PERSISTENT);

                    //创建 Ip+port 节点，为临时性节点
                    IPAddress[] IPList = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList;
                    string currentIp = IPList.Where(ip => ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).Last().ToString();
                    if (zooKeeper.existsAsync($@"{MyApp}/{item.Key}/{apiPath}/{currentIp}:8080") != null)
                        zooKeeper.createAsync($@"{MyApp}/{item.Key}/{apiPath}/{currentIp}:8080", null, Ids.OPEN_ACL_UNSAFE, CreateMode.EPHEMERAL);
                }
            }

        }
    }
}
