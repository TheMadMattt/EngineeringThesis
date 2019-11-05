using System.Collections.Generic;
using System.Linq;
using EngineeringThesis.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace EngineeringThesis.Core.Services
{
    public class CustomerService
    {
        private readonly ApplicationContext _ctx;

        public CustomerService(ApplicationContext context)
        {
            _ctx = context;
        }
        public List<Customer> GetContractors()
        {
            return _ctx.Customers.Where(x => x.CustomerTypeId == 1).Include(customerType => customerType.CustomerType)
                .ToList();
        }

        public List<Customer> GetSellers()
        {
            return _ctx.Customers.Where(x => x.CustomerTypeId == 2).Include(customerType => customerType.CustomerType)
                .ToList();
        }

        public void SaveCustomer(Customer customerWithRef)
        {
            _ctx.Customers.Add(customerWithRef);
            _ctx.SaveChanges();
        }

        public void UpdateCustomer(Customer customerWithRef)
        {
            var oldCustomer = _ctx.Customers.Find(customerWithRef.Id);

            _ctx.Entry(oldCustomer).CurrentValues.SetValues(customerWithRef);
            _ctx.SaveChanges();
        }

        public List<CustomerType> GetCustomerTypes()
        {
            return _ctx.CustomerTypes.ToList();
        }

        public void DeleteCustomer(Customer customer)
        {
            _ctx.Customers.Remove(customer);
            _ctx.SaveChanges();
        }

        public Customer GetCustomer(int id)
        {
            return _ctx.Customers
                .Include(x => x.CustomerType)
                .FirstOrDefault(x => x.Id == id);
        }
    }
}
