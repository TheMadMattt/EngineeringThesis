using System;
using System.Globalization;
using EngineeringThesis.Core.Models;
using EngineeringThesis.Core.Models.DisplayModels;

namespace EngineeringThesis.Core.ViewModel
{
    public class InvoiceItemViewModel : BaseViewModel
    {
        private InvoiceItemDisplayModel _invoiceItem;
        public InvoiceItem InvoiceItemWithRef;

        public InvoiceItemViewModel() {}

        public InvoiceItemDisplayModel InvoiceItem
        {
            get
            {
                if (_invoiceItem != null)
                {
                    return _invoiceItem;
                }

                return _invoiceItem = new InvoiceItemDisplayModel();
            }
            set => SetProperty(ref _invoiceItem, value);
        }

        public void BindToRefObject()
        {
            InvoiceItemWithRef.Name = InvoiceItem.Name;
            InvoiceItemWithRef.PKWiU = InvoiceItem.PKWiU;
            InvoiceItemWithRef.Unit = InvoiceItem.Unit;
            InvoiceItemWithRef.NetPrice = FormatCurrency(InvoiceItem.NetPrice);
            InvoiceItemWithRef.Amount = InvoiceItem.Amount;
            InvoiceItemWithRef.VAT = Convert.ToInt16(InvoiceItem.VAT);
            InvoiceItemWithRef.NetSum = FormatCurrency(InvoiceItem.NetSum);
            InvoiceItemWithRef.GrossSum = FormatCurrency(InvoiceItem.GrossSum);
            InvoiceItemWithRef.Comments = InvoiceItem.Comments;
        }

        public string FormatCurrency(string number)
        {
            if (number.Contains("."))
            {
                number = number.Replace(".", ",");
            }
            var clone = (CultureInfo)CultureInfo.InvariantCulture.Clone();
            clone.NumberFormat.NumberDecimalSeparator = ",";
            clone.NumberFormat.NumberGroupSeparator = ".";

            return decimal.Parse(number, clone).ToString("#.00", new CultureInfo("pl"));
        }
    }
}
