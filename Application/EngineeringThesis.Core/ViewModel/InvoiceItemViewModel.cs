using System;
using System.Globalization;
using EngineeringThesis.Core.Models;
using EngineeringThesis.Core.Services;

namespace EngineeringThesis.Core.ViewModel
{
    public class InvoiceItemViewModel: BaseViewModel
    {
        private readonly InvoiceItemService _invoiceItemService;
        private InvoiceItem _invoiceItem;
        public InvoiceItem InvoiceItemWithRef;

        public InvoiceItemViewModel(InvoiceItemService invoiceItemService)
        {
            _invoiceItemService = invoiceItemService;
        }

        public InvoiceItem InvoiceItem
        {
            get
            {
                if (_invoiceItem != null)
                {
                    return _invoiceItem;
                }

                return _invoiceItem = new InvoiceItem();
            }
            set => SetProperty(ref _invoiceItem, value);
        }

        public string FormatCurrency(string number)
        {
            var clone = (CultureInfo)CultureInfo.InvariantCulture.Clone();
            clone.NumberFormat.NumberDecimalSeparator = ",";
            clone.NumberFormat.NumberGroupSeparator = ".";

            return decimal.Parse(number, clone).ToString("#.00");
        }

        public void BindToRefObject()
        {
            try
            {
                InvoiceItemWithRef.Name = InvoiceItem.Name;
                InvoiceItemWithRef.PKWiU = InvoiceItem.PKWiU;
                InvoiceItemWithRef.Unit = InvoiceItem.Unit;
                InvoiceItemWithRef.NetPrice =
                    FormatCurrency(InvoiceItem.NetPrice);
                InvoiceItemWithRef.Amount = InvoiceItem.Amount;
                InvoiceItemWithRef.VAT = InvoiceItem.VAT;
                InvoiceItemWithRef.NetSum = InvoiceItem.NetSum;
                InvoiceItemWithRef.GrossSum = InvoiceItem.GrossSum;
                InvoiceItemWithRef.Comments = InvoiceItem.Comments;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
