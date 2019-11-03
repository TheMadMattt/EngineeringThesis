using System;
using System.Collections.Generic;
using EngineeringThesis.Core.Models;
using EngineeringThesis.Core.Services;

namespace EngineeringThesis.Core.ViewModel
{
    public class InvoiceViewModel : BaseViewModel
    {
        private readonly CustomerService _customerService;
        private readonly InvoiceService _invoiceService;
        private readonly PaymentTypeService _paymentTypeService;
        private Invoice _invoice;

        public Invoice InvoiceWithRef;
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

        public Invoice Invoice
        {
            get
            {
                if (_invoice != null)
                {
                    return _invoice;
                }
                else
                {
                    return _invoice = new Invoice();
                }
            }
            set => SetProperty(ref _invoice, value);
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
            return !string.IsNullOrEmpty(obj.Name) && obj.Amount > 0 && !string.IsNullOrEmpty(obj.Unit)
                   && !string.IsNullOrEmpty(obj.NetPrice) && obj.VAT > 0 && !string.IsNullOrEmpty(obj.VATSum)
                   && !string.IsNullOrEmpty(obj.NetSum) && !string.IsNullOrEmpty(obj.GrossSum);
        }

        public void BindData(Invoice invoice)
        {
            if (!string.IsNullOrEmpty(invoice.InvoiceNumber))
            {
                Invoice.InvoiceNumber = invoice.InvoiceNumber;
            }
            if (!string.IsNullOrEmpty(invoice.InvoiceDate.ToShortDateString()))
            {
                Invoice.InvoiceDate = invoice.InvoiceDate;
            }
            if (!string.IsNullOrEmpty(invoice.PaymentDeadline.ToShortDateString()))
            {
                Invoice.PaymentDeadline = invoice.PaymentDeadline;
            }
            if (!string.IsNullOrEmpty(invoice.PaymentDate?.ToShortDateString()))
            {
                Invoice.PaymentDate = invoice.PaymentDate;
            }

            if (invoice.InvoiceItems != null && invoice.InvoiceItems.Count > 0)
            {
                Invoice.InvoiceItems = invoice.InvoiceItems;
            }
            else
            {
                Invoice.InvoiceItems = new List<InvoiceItem>();
            }
            Invoice.ContractorId = invoice.ContractorId;
            Invoice.SellerId = invoice.SellerId;
            Invoice.PaymentTypeId = invoice.PaymentTypeId;
            Invoice.Comments = invoice.Comments;
        }
    }
}
