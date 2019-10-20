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

        public Invoice GetLastInvoice()
        {
            return _invoiceService.GetLastInvoice();
        }

        public string CreateInvoiceNumber(string lastInvoiceNumber)
        {
            string[] splitted = lastInvoiceNumber.Split("/");
            var number = Convert.ToInt32(splitted[0]);
            var year = Convert.ToInt32(splitted[1]);

            number += 1;
            if (!year.Equals(DateTime.Today.Year))
            {
                year += 1;
            }

            return number + "/" + year;
        }

        public static bool IsNullOrEmpty(InvoiceItem obj)
        {
            return !String.IsNullOrEmpty(obj.Name) && obj.Amount > 0 && !String.IsNullOrEmpty(obj.Unit)
                   && !String.IsNullOrEmpty(obj.NetPrice) && obj.VAT > 0 && !String.IsNullOrEmpty(obj.VATSum)
                   && !String.IsNullOrEmpty(obj.NetSum) && !String.IsNullOrEmpty(obj.GrossSum);
        }
    }
}
