using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Navigation;
using EngineeringThesis.Core.Models;

namespace EngineeringThesis.UI.ViewModel
{
    public class InvoiceItemViewModel: BaseViewModel
    {
        private InvoiceItem _invoiceItem;
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

        private InvoiceItem _invoiceItemWithRef;

        public InvoiceItem InvoiceItemWithRef
        {
            get
            {
                if (_invoiceItemWithRef != null)
                {
                    return _invoiceItemWithRef;
                }

                return _invoiceItemWithRef = new InvoiceItem();
            }
            set => SetProperty(ref _invoiceItemWithRef, value);
        }
    }
}
