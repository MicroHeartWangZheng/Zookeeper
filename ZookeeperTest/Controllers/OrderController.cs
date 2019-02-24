using Domain;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using org.apache.zookeeper;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Watch;

namespace OrderMicroService.Controllers
{
    public class OrderController : ControllerBase
    {
        [Route("Order/Order")]
        public string Order(Order order)
        {
            return "下单成功!";
        }
        [Route("Order/GetOrders")]
        public async Task<List<Order>> GetOrders()
        {
            List<Order> orders = new List<Order>();

            Order order = null;
            HttpClient client = new HttpClient();
            for (var i = 0; i < 10; i++)
            {
                order = new Order();
                order.Address = "浙江省杭州市拱墅区北部软件园" + i;
                order.CustomerId = i;
                order.Goods = "麻辣香锅" + i;
                order.Id = i;

                ZooKeeper zooKeeper = new ZooKeeper("127.0.0.1:2181", 50000, new MyWatcher());

                ChildrenResult childrenResult1 = await zooKeeper.getChildrenAsync("/MyApp/CustomerServices");
                ChildrenResult childrenResult = await zooKeeper.getChildrenAsync("/MyApp/CustomerServices/Customer-GetCustomer");
                

                //生成一个随机数 
                Random random = new Random();
                var num = random.Next(0, childrenResult.Children.Count - 1);
                //通过随机数 获取服务下随机的一个地址 
                var url = $@"{ childrenResult.Children[num]}/Customer/GetCustomer?Id=" + order.CustomerId;

                var result = await client.GetAsync(url);

                Custormer custormer = JsonConvert.DeserializeObject<Custormer>(result.Content.ReadAsStringAsync().Result);

                order.Custormer = custormer;

                orders.Add(order);
            }

            return orders;
        }
    }
}