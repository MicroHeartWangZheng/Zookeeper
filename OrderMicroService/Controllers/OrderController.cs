﻿using Domain;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using org.apache.zookeeper;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Watch;
using static org.apache.zookeeper.ZooDefs;

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
                //连接ZooKeeper
                ZooKeeper zooKeeper = new ZooKeeper("118.24.96.212:2181", 50000, new MyWatcher());

                ChildrenResult childrenResult = null;

                if (await zooKeeper.existsAsync("/MyApp/CustomerServices/Customer-GetCustomer") != null)
                    childrenResult = await zooKeeper.getChildrenAsync("/MyApp/CustomerServices/Customer-GetCustomer");

                //生成一个随机数 
                Random random = new Random();
                var num = random.Next(0, childrenResult.Children.Count - 1);

                //通过随机数 获取服务下随机的一个地址 
                var url = $@"http://{childrenResult.Children[num]}/Customer/GetCustomer?Id=" + order.CustomerId;
                var result = await client.GetAsync(url);

                //通过地址获取顾客的信息
                Custormer custormer = JsonConvert.DeserializeObject<Custormer>(result.Content.ReadAsStringAsync().Result);
                order.Custormer = custormer;

                orders.Add(order);
            }

            return orders;
        }
    }
}