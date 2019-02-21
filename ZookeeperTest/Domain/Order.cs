using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZookeeperTest.Domain
{
    public class Order
    {
        public int Id { get; set; }

        public string CustomerName { get; set; }

        public string Goods { get; set; }

        public string Address { get; set; }
    }
}
