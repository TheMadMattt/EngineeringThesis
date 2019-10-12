using System.Collections.Generic;
using EngineeringThesis.Core.Models;
using EngineeringThesis.Core.Services;

namespace EngineeringThesis.UI.ViewModel
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
