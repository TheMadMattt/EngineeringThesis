using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
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

        public void SaveCustomer(Customer customerWithRef)
        {
            using var ctx = new ApplicationContext();

            ctx.Customers.Add(customerWithRef);
            ctx.SaveChanges();
        }

        public void UpdateCustomer(Customer customerWithRef)
        {
            using var ctx = new ApplicationContext();

            var oldCustomer = ctx.Customers.Find(customerWithRef.Id);

            ctx.Entry(oldCustomer).CurrentValues.SetValues(customerWithRef);
            ctx.SaveChanges();
        }

        public List<CustomerType> GetCustomerTypes()
        {
            using var ctx = new ApplicationContext();

            return ctx.CustomerTypes.ToList();
        }

        public void DeleteCustomer(Customer customer)
        {
            using var ctx = new ApplicationContext();

            ctx.Customers.Remove(customer);
            ctx.SaveChanges();
        }
    }
}
