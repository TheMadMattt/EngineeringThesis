using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using EngineeringThesis.Core.Models;
using EngineeringThesis.UI.Navigation;
using EngineeringThesis.UI.ViewModel;

namespace EngineeringThesis.UI.View
{
    /// <summary>
    /// Interaction logic for InvoiceWindow.xaml
    /// </summary>
    public partial class InvoiceWindow : Window, IActivable
    {
        private readonly NavigationService _navigationService;
        public InvoiceViewModel InvoiceViewModel;
        public InvoiceWindow(NavigationService navigationService, InvoiceViewModel invoiceViewModel)
        {
            Language = XmlLanguage.GetLanguage("pl-PL");
            InitializeComponent();
            InvoiceDatePicker.SelectedDate = DateTime.Today;
            InvoiceViewModel = invoiceViewModel;
            _navigationService = navigationService;

            ContractorComboBox.ItemsSource = InvoiceViewModel.GetContractors();
            SellerComboBox.ItemsSource = InvoiceViewModel.GetSellers();
            PaymentTypeComboBox.ItemsSource = InvoiceViewModel.GetPaymentTypes();
        }

        public Task ActivateAsync(object parameter)
        {
            if (parameter is Invoice invoice)
            {
                InvoiceViewModel.Invoice = invoice;
                BindInvoiceToControls();
            }
            
            return Task.CompletedTask;
        }

        public void BindInvoiceToControls()
        {
            ContractorComboBox.SelectedItem = InvoiceViewModel.Contractors.Find(x => x.Id == InvoiceViewModel.Invoice.ContractorId);
            SellerComboBox.SelectedItem = InvoiceViewModel.Sellers.Find(x => x.Id == InvoiceViewModel.Invoice.SellerId);
            InvoiceDatePicker.SelectedDate = InvoiceViewModel.Invoice.InvoiceDate;
            PaymentTypeComboBox.SelectedItem = InvoiceViewModel.PaymentTypes.Find(x => x.Id == InvoiceViewModel.Invoice.PaymentTypeId);
            PaymentDeadlineDatePicker.SelectedDate = InvoiceViewModel.Invoice.PaymentDeadline;
            if (InvoiceViewModel.Invoice.PaymentDate.HasValue)
            {
                IsPaidCheckBox.IsChecked = true;
                PaidDatePicker.SelectedDate = InvoiceViewModel.Invoice.PaymentDate;
            }

            InvoiceItemsDataGrid.ItemsSource = InvoiceViewModel.Invoice.InvoiceItems;
        }
    }
}
