using System.Collections.Generic;
using EngineeringThesis.Core.Models;
using EngineeringThesis.Core.Models.DisplayModels;
using EngineeringThesis.Core.Services;

namespace EngineeringThesis.Core.ViewModel
{
    public class AddCustomerViewModel: BaseViewModel
    {
        private readonly CustomerService _customerService;
        private CustomerDisplayModel _customer;
        public Customer CustomerWithRef;
        public List<CustomerType> CustomerTypes;

        public AddCustomerViewModel(CustomerService customerService)
        {
            _customerService = customerService;
        }

        public CustomerDisplayModel Customer
        {
            get
            {
                if (_customer != null)
                {
                    return _customer;
                }

                return _customer = new CustomerDisplayModel();
            }
            set => SetProperty(ref _customer, value);
        }

        public void SaveCustomer()
        {
            _customerService.SaveCustomer(CustomerWithRef);
        }
        public void UpdateCustomer()
        {
            _customerService.UpdateCustomer(CustomerWithRef);
        }

        public List<CustomerType> GetCustomerTypes()
        {
            return CustomerTypes = _customerService.GetCustomerTypes();
        }

        public bool IsUpdate { get; set; } = false;

        public void SplitAddress(string address)
        {
            string[] numbers = address.Split("/");
            if (numbers.Length == 2)
            {
                Customer.StreetNumber = numbers[0];
                Customer.FlatNumber = numbers[1];
            }
            else
            {
                Customer.StreetNumber = numbers[0];
            }
            
        }

        public void BindToRefObject()
        {
            CustomerWithRef.Name = Customer.Name;
            CustomerWithRef.ZipCode = Customer.ZipCode;
            CustomerWithRef.City = Customer.City;
            CustomerWithRef.Street = Customer.Street;
            if (!string.IsNullOrEmpty(Customer.StreetNumber))
            {
                if (!string.IsNullOrEmpty(Customer.FlatNumber))
                {
                    CustomerWithRef.StreetNumber = Customer.StreetNumber + "/" + Customer.FlatNumber;
                }
                else
                {
                    CustomerWithRef.StreetNumber = Customer.StreetNumber;
                }
            }
            CustomerWithRef.PhoneNumber = Customer.PhoneNumber;
            CustomerWithRef.NIP = Customer.NIP;
            CustomerWithRef.REGON = Customer.REGON;
            CustomerWithRef.BankAccountNumber = Customer.BankAccountNumber;
            CustomerWithRef.Comments = Customer.Comments;
        }

        public void BindData(Customer customer)
        {
            Customer = new CustomerDisplayModel();
            if (!string.IsNullOrEmpty(customer.Name))
            {
                Customer.Name = customer.Name;
            }
            if (!string.IsNullOrEmpty(customer.ZipCode))
            {
                Customer.ZipCode = customer.ZipCode;
            }
            if (!string.IsNullOrEmpty(customer.City))
            {
                Customer.City = customer.City;
            }
            if (!string.IsNullOrEmpty(customer.Street))
            {
                Customer.Street = customer.Street;
            }
            if (!string.IsNullOrEmpty(customer.PhoneNumber))
            {
                Customer.PhoneNumber = customer.PhoneNumber;
            }
            if (!string.IsNullOrEmpty(customer.NIP))
            {
                Customer.NIP = customer.NIP;
            }
            if (!string.IsNullOrEmpty(customer.REGON))
            {
                Customer.REGON = customer.REGON;
            }
            if (!string.IsNullOrEmpty(customer.BankAccountNumber))
            {
                Customer.BankAccountNumber = customer.BankAccountNumber;
            }

            Customer.Comments = customer.Comments;
            Customer.CustomerTypeId = customer.CustomerTypeId;
        }
    }
}
