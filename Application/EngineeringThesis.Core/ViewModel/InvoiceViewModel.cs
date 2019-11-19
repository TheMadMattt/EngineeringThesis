using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Navigation;
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
        private Customer _selectedSeller;
        private Customer _selectedContractor;

        public Invoice InvoiceWithRef;
        public Invoice LastInvoice;
        public List<PaymentType> PaymentTypes;
        public ObservableCollection<Customer> Contractors;
        public ObservableCollection<Customer> Sellers;
        public int InvoiceYear;
        public int InvoiceNumber;
        public InvoiceViewModel(CustomerService customerService, InvoiceService invoiceService, 
                                PaymentTypeService paymentTypeService)
        {
            _customerService = customerService;
            _invoiceService = invoiceService;
            _paymentTypeService = paymentTypeService;
        }

        public ObservableCollection<Customer> GetContractors()
        {
            return Contractors = new ObservableCollection<Customer>(_customerService.GetContractors());
        }

        public ObservableCollection<Customer> GetSellers()
        {
            return Sellers = new ObservableCollection<Customer>(_customerService.GetSellers());
        }

        public List<PaymentType> GetPaymentTypes()
        {
            return PaymentTypes = _paymentTypeService.GetPaymentTypes();
        }

        public Invoice GetLastInvoice()
        {
            return LastInvoice = _invoiceService.GetLastInvoice();
        }

        public Invoice GetInvoice()
        {
            if (Invoice.Id > 0)
            {
                return _invoiceService.GetInvoice(Invoice.Id);
            }
            if (Invoice.ContractorId > 0 && Invoice.SellerId > 0)
            {
                Invoice.Contractor = _customerService.GetCustomer(Invoice.ContractorId);
                Invoice.Seller = _customerService.GetCustomer(Invoice.SellerId);
            }
            if (Invoice.PaymentTypeId > 0)
            {
                Invoice.PaymentType = _paymentTypeService.GetPaymentType(Invoice.PaymentTypeId);
            }

            return Invoice;
        }

        public Invoice Invoice
        {
            get
            {
                if (_invoice != null)
                {
                    return _invoice;
                }

                return _invoice = new Invoice();
            }
            set => SetProperty(ref _invoice, value);
        }

        public Customer SelectedContractor
        {
            get => _selectedContractor;
            set => SetProperty(ref _selectedContractor, value);
        }

        public Customer SelectedSeller
        {
            get => _selectedSeller;
            set => SetProperty(ref _selectedSeller, value);
        }

        public bool IsUpdate { get; set; }

        public string CreateInvoiceNumber(string? lastInvoiceNumber)
        {
            if (lastInvoiceNumber != null)
            {
                string[] splitted = lastInvoiceNumber.Split("/");
                InvoiceNumber = Convert.ToInt32(splitted[0]);
                InvoiceYear = Convert.ToInt32(splitted[1]);

                InvoiceNumber += 1;
                if (InvoiceYear < DateTime.Today.Year)
                {
                    InvoiceYear += 1;
                    InvoiceNumber = 1;
                }
            }
            else
            {
                InvoiceNumber = 1;
                InvoiceYear = DateTime.Today.Year;
            }
            

            var newLastInvoiceNumber = InvoiceNumber + "/" + InvoiceYear;

            return newLastInvoiceNumber;
        }

        public void SaveLastInvoiceNumber(string newLastInvoiceNumber)
        {
            var lastInvoiceYear = Convert.ToInt32(newLastInvoiceNumber.Split("/")[1]);
            var lastNumber = _invoiceService.FindLastInvoiceNumber(lastInvoiceYear.ToString());

            if(lastNumber != null)
            {
                lastNumber.InvoiceNumber = newLastInvoiceNumber;

                _invoiceService.UpdateLastInvoiceNumber(lastNumber);
            }
            else
            {
                var newLastNumber = new LastInvoiceNumber
                {
                    InvoiceNumber = newLastInvoiceNumber
                };

                _invoiceService.SaveLastInvoiceNumber(newLastNumber);
            }
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
            Invoice.IsProformaInvoice = invoice.IsProformaInvoice;
            Invoice.Comments = invoice.Comments;
        }

        public void BindDataToRef()
        {
            InvoiceWithRef.InvoiceNumber = Invoice.InvoiceNumber;
            InvoiceWithRef.InvoiceDate = Invoice.InvoiceDate;
            InvoiceWithRef.PaymentDeadline = Invoice.PaymentDeadline;
            InvoiceWithRef.PaymentDate = Invoice.PaymentDate;
            InvoiceWithRef.InvoiceItems = Invoice.InvoiceItems;
            InvoiceWithRef.ContractorId = Invoice.Contractor.Id;
            InvoiceWithRef.Contractor = Invoice.Contractor;
            InvoiceWithRef.SellerId = Invoice.Seller.Id;
            InvoiceWithRef.Seller = Invoice.Seller;
            InvoiceWithRef.PaymentType = Invoice.PaymentType;
            InvoiceWithRef.PaymentTypeId = Invoice.PaymentTypeId;
            InvoiceWithRef.IsProformaInvoice = Invoice.IsProformaInvoice;
            InvoiceWithRef.Comments = Invoice.Comments;
        }

        public void SaveInvoice()
        {
            if (IsUpdate)
            {
                _invoiceService.UpdateInvoice(InvoiceWithRef);
            }
            else
            {
                _invoiceService.SaveInvoice(InvoiceWithRef);
            }
            
        }

        public LastInvoiceNumber GetLastInvoiceNumber(int valueYear)
        {
            var lastInvoiceNumber = _invoiceService.FindLastInvoiceNumber(valueYear.ToString());

            return lastInvoiceNumber;
        }
    }
}
