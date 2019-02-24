using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace CustomerMicroService.Controllers
{
    public class CustomerController : ControllerBase
    {
        [Route("Customer/GetCustomer")]
        public Custormer GetCustomer(int Id)
        {
            return new Custormer() { Id=Id,Name="MicroHeart"+Id,Phone="1234567"};
        }
    }
}