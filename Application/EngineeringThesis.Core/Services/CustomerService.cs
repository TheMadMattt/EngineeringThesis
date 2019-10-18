using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EngineeringThesis.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace EngineeringThesis.Core.Services
{
    public class CustomerService
    {
        public List<Customer> GetContractors()
        {
            using var ctx = new ApplicationContext();

            return ctx.Customers.Where(x => x.CustomerTypeId == 1).Include(customerType => customerType.CustomerType)
                .ToList();
        }

        public List<Customer> GetSellers()
        {
            using var ctx = new ApplicationContext();

            return ctx.Customers.Where(x => x.CustomerTypeId == 2).Include(customerType => customerType.CustomerType)
                .ToList();
        }
    }
}
