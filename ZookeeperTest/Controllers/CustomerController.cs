using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ZookeeperTest.Domain;

namespace ZookeeperTest.Controllers
{
    public class CustomerController : ControllerBase
    {
        [Route("Customer/GetCustormer")]
        public Custormer GetCustormer(int Id)
        {
            return new Custormer() { Id=Id,Name="Microheart"+Id,Phone="1234567"};
        }
    }
}