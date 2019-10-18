using System;
using System.Collections.Generic;
using System.Text;
using EngineeringThesis.Core.Models;
using EngineeringThesis.Core.Services;

namespace EngineeringThesis.UI.ViewModel
{
    public class InvoiceViewModel : BaseViewModel
    {
        private readonly CustomerService _customerService;
        private readonly InvoiceService _invoiceService;
        private readonly PaymentTypeService _paymentTypeService;

        public Invoice Invoice;
        public List<PaymentType> PaymentTypes;
        public List<Customer> Contractors;
        public List<Customer> Sellers;
        public InvoiceViewModel(CustomerService customerService, InvoiceService invoiceService, 
                                PaymentTypeService paymentTypeService)
        {
            _customerService = customerService;
            _invoiceService = invoiceService;
            _paymentTypeService = paymentTypeService;
        }

        public List<Customer> GetContractors()
        {
            return Contractors = _customerService.GetContractors();
        }

        public List<Customer> GetSellers()
        {
            return Sellers = _customerService.GetSellers();
        }

        public List<PaymentType> GetPaymentTypes()
        {
            return PaymentTypes = _paymentTypeService.GetPaymentTypes();
        }

        public Invoice GetInvoice(int id)
        {
            return _invoiceService.GetInvoice(id);
        }
    }
}
