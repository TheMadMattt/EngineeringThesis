using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using EngineeringThesis.Core.Models;
using EngineeringThesis.Core.Services;

namespace EngineeringThesis.Core.ViewModel
{
    public class MainViewModel: BaseViewModel
    {
        private readonly InvoiceService _invoiceService;
        public List<Invoice> Invoices;

        public MainViewModel(InvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        public List<Invoice> GetInvoices()
        {
            return Invoices = _invoiceService.GetInvoices();
        }
    }
}
