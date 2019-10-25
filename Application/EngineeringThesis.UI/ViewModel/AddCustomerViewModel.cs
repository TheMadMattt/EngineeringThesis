using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using EngineeringThesis.Core.Models;
using EngineeringThesis.Core.Services;

namespace EngineeringThesis.UI.ViewModel
{
    public class AddCustomerViewModel : BaseViewModel
    {
        private readonly CustomerService _customerService;
        private Customer _customer;
        private string _flatNumber;
        private string _streetNumber;
        private bool _isUpdate = false;

        public Customer CustomerWithRef;
        public List<CustomerType> CustomerTypes;

        public AddCustomerViewModel(CustomerService customerService)
        {
            _customerService = customerService;
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

        public Customer Customer
        {
            get
            {
                if (_customer != null)
                {
                    return _customer;
                }

                return _customer = new Customer();
            }
            set => SetProperty(ref _customer, value);
        }

        public bool IsUpdate
        {
            get => _isUpdate;
            set => SetProperty(ref _isUpdate, value);
        }

        public string FlatNumber
        {
            get => _flatNumber;
            set => SetProperty(ref _flatNumber, value);
        }

        public string StreetNumber
        {
            get => _streetNumber;
            set => SetProperty(ref _streetNumber, value);
        }

        public void SplitAddress(string address)
        {
            string[] numbers = address.Split("/");
            if (numbers.Length == 2)
            {
                StreetNumber = numbers[0];
                FlatNumber = numbers[1];
            }
            else
            {
                StreetNumber = numbers[0];
            }
            
        }

        public void BindToRefObject()
        {
            CustomerWithRef.Name = Customer.Name;
            CustomerWithRef.ZipCode = Customer.ZipCode;
            CustomerWithRef.City = Customer.City;
            CustomerWithRef.Street = Customer.Street;
            if (!string.IsNullOrEmpty(StreetNumber))
            {
                if (!string.IsNullOrEmpty(FlatNumber))
                {
                    CustomerWithRef.StreetNumber = StreetNumber + "/" + FlatNumber;
                }
                else
                {
                    CustomerWithRef.StreetNumber = StreetNumber;
                }
            }
            CustomerWithRef.PhoneNumber = Customer.PhoneNumber;
            CustomerWithRef.NIP = Customer.NIP;
            CustomerWithRef.REGON = Customer.REGON;
            CustomerWithRef.BankAccountNumber = Customer.BankAccountNumber;
            CustomerWithRef.Comments = Customer.Comments;
            CustomerWithRef.CustomerTypeId = 1;
        }
    }
}
