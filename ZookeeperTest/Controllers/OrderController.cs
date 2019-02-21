using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ZookeeperTest.Domain;

namespace ZookeeperTest.Controllers
{
    public class OrderController : Controller
    {

        public string Order(Order order)
        {
            return "下单成功!";
        }

        public List<Order> GetOrders()
        {
            List<Order> orders = new List<Order>();

            Order order = null;
            for (var i = 0; i < 10; i++)
            {
                order = new Order();
                order.Address = "浙江省杭州市拱墅区北部软件园" + i;
                order.CustomerName = "MicroHeart";
                order.Goods = "麻辣香锅" + i;
                order.Id = i;
                orders.Add(order);
            }
            return orders;
        }
    }
}