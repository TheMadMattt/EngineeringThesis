using System;
using System.Collections;
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
        public List<Customer> Contractors;
        public List<Customer> Sellers;

        public MainViewModel(InvoiceService invoiceService, CustomerService customerService)
        {
            _invoiceService = invoiceService;
            _customerService = customerService;
        }

        public List<Invoice> GetInvoices()
        {
            return Invoices = _invoiceService.GetInvoices();
        }

        public List<Customer> GetContractors()
        {
            return Contractors = _customerService.GetContractors();
        }

        public void DeleteInvoice(Invoice invoice)
        {
            var lastInvoiceNumber = _invoiceService.FindLastInvoiceNumber(invoice.InvoiceNumber);
            if (lastInvoiceNumber != null)
            {
                string[] split = lastInvoiceNumber.InvoiceNumber.Split("/");
                var number = Convert.ToInt32(split[0]);
                var year = Convert.ToInt32(split[1]);
                if (number > 1)
                {
                    number -= 1;
                    lastInvoiceNumber.InvoiceNumber = number + "/" + year;

                    _invoiceService.UpdateLastInvoiceNumber(lastInvoiceNumber);
                }

                if (number == 1)
                {
                    _invoiceService.DeleteLastInvoiceNumber(lastInvoiceNumber);
                }

            }
            _invoiceService.DeleteInvoice(invoice);
        }

        public void DeleteCustomer(Customer customer)
        {
            _customerService.DeleteCustomer(customer);
        }

        public IEnumerable GetSellers()
        {
            return Sellers = _customerService.GetSellers();
        }
    }
}
