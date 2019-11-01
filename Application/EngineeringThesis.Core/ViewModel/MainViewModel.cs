using System.Collections.Generic;
using EngineeringThesis.Core.Models;
using EngineeringThesis.Core.Services;

namespace EngineeringThesis.Core.ViewModel
{
    public class MainViewModel: BaseViewModel
    {
        private readonly InvoiceService _invoiceService;
        private readonly CustomerService _customerService;
        public List<Invoice> Invoices;
        public List<Customer> Customers;

        public MainViewModel(InvoiceService invoiceService, CustomerService customerService)
        {
            _invoiceService = invoiceService;
            _customerService = customerService;
        }

        public List<Invoice> GetInvoices()
        {
            return Invoices = _invoiceService.GetInvoices();
        }

        public List<Customer> GetCustomers()
        {
            return Customers = _customerService.GetContractors();
        }

        public void DeleteInvoice(Invoice invoice)
        {
            _invoiceService.DeleteInvoice(invoice);
        }

        public void DeleteCustomer(Customer customer)
        {
            _customerService.DeleteCustomer(customer);
        }

        public static bool IsNullOrEmpty(Customer obj)
        {
            return !string.IsNullOrEmpty(obj.Name) && !string.IsNullOrEmpty(obj.ZipCode)
                   && !string.IsNullOrEmpty(obj.City) && !string.IsNullOrEmpty(obj.Street)
                   && !string.IsNullOrEmpty(obj.StreetNumber);
        }
    }
}
