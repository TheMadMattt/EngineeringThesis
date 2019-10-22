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
        public Customer CustomerWithRef;
        private Customer _customer;
        private string _flatNumber;
        private string _streetNumber;
        public AddCustomerViewModel(CustomerService customerService)
        {
            _customerService = customerService;
        }

        public bool SaveCustomer()
        {
            return _customerService.SaveCustomer(CustomerWithRef);
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

        public StreetFlatNumber SplitAddress(string address)
        {
            StreetFlatNumber streetFlatNumber = new StreetFlatNumber();

            string[] numbers = address.Split("/");
            streetFlatNumber.StreetNumber = numbers[0];
            streetFlatNumber.FlatNumber = numbers[1];

            return streetFlatNumber;
        }

        public struct StreetFlatNumber
        {
            public string FlatNumber;
            public string StreetNumber;
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
