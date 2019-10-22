using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using EngineeringThesis.Core.Models;
using EngineeringThesis.Core.Utility.ShowDialogs;
using EngineeringThesis.UI.Navigation;
using EngineeringThesis.UI.ViewModel;
using Forge.Forms;
using MaterialDesignThemes.Wpf;

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
                InvoiceViewModel.Invoice = InvoiceViewModel.GetInvoice(invoice.Id);
                BindInvoiceToControls();
            }
            else
            {
                InvoiceViewModel.Invoice = new Invoice();
                var lastInvoice = InvoiceViewModel.GetLastInvoice();
                InvoiceViewModel.Invoice.InvoiceNumber =
                    InvoiceViewModel.CreateInvoiceNumber(lastInvoice.InvoiceNumber);
                InvoiceViewModel.Invoice.InvoiceItems = new List<InvoiceItem>();
                BindNewInvoiceToControls();
            }
            

            return Task.CompletedTask;
        }

        private void BindNewInvoiceToControls()
        {
            ContractorComboBox.SelectedItem = InvoiceViewModel.Contractors[0];
            SellerComboBox.SelectedItem = InvoiceViewModel.Sellers[0];
            InvoiceDatePicker.SelectedDate = DateTime.Today;
            PaymentTypeComboBox.SelectedItem = InvoiceViewModel.PaymentTypes[0];
            PaymentDeadlineDatePicker.SelectedDate = DateTime.Today;
            IsPaidCheckBox.IsChecked = false;
            InvoiceItemsDataGrid.ItemsSource = InvoiceViewModel.Invoice.InvoiceItems;
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

        private async void DeleteItemBtn_Click(object sender, RoutedEventArgs e)
        {
            if (InvoiceItemsDataGrid.SelectedItem != null)
            {
                var result = await Forge.Forms.Show.Dialog("InvoiceDialogHost").For(new Warning("Czy napewno chcesz usunąć: " + ((InvoiceItem)InvoiceItemsDataGrid.SelectedItem).Name,
                    "Usuwanie produktu", "Tak", "Nie"));
                if (result.Action != null)
                {
                    if (result.Action.Equals("positive"))
                    {
                        InvoiceViewModel.Invoice.InvoiceItems.Remove((InvoiceItem)InvoiceItemsDataGrid.SelectedItem);
                        InvoiceItemsDataGrid.Items.Refresh();
                    }
                }
            }
            else
            {
                await Forge.Forms.Show.Dialog("InvoiceDialogHost").For(new Information("Żaden produkt nie został wybrany", "Zaznacz produkt", "OK"));
            }
        }

        private async void AddItemBtn_Click(object sender, RoutedEventArgs e)
        {
            InvoiceItem invoiceItem = new InvoiceItem();
            var invoiceItemWindow = await _navigationService.ShowDialogAsync<InvoiceItemWindow>(invoiceItem);
            if (InvoiceViewModel.IsNullOrEmpty(invoiceItem))
            {
                InvoiceViewModel.Invoice.InvoiceItems.Add(invoiceItem);
                InvoiceItemsDataGrid.Items.Refresh();
            }
            
        }

        private async void EditItemBtn_Click(object sender, RoutedEventArgs e)
        {
            if (InvoiceItemsDataGrid.SelectedItem is InvoiceItem invoiceItem)
            {
                var invoiceItemWindow = await _navigationService.ShowDialogAsync<InvoiceItemWindow>(invoiceItem);

                if (InvoiceViewModel.IsNullOrEmpty(invoiceItem))
                {
                    var invoiceItemsList = InvoiceViewModel.Invoice.InvoiceItems.ToList();
                    var index = invoiceItemsList.FindIndex(x => x.Id == invoiceItem.Id);
                    invoiceItemsList[index] = invoiceItem;
                }
                
            }
            else
            {
                await Forge.Forms.Show.Dialog("InvoiceDialogHost").For(new Information("Żaden produkt nie został wybrany", "Zaznacz produkt", "OK"));
            }
            InvoiceItemsDataGrid.Items.Refresh();
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;

        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MaximizeWindow_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Normal)
            {
                WindowState = WindowState.Maximized;
                MaximizeWindowIcon.Kind = PackIconKind.WindowRestore;
            }
            else
            {
                WindowState = WindowState.Normal;
                MaximizeWindowIcon.Kind = PackIconKind.WindowMaximize;
            }
        }

        private void ToolbarGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (WindowState == WindowState.Normal)
            {
                MaximizeWindowIcon.Kind = PackIconKind.WindowMaximize;
            }
            else
            {
                MaximizeWindowIcon.Kind = PackIconKind.WindowRestore;
            }
        }

        private void InvoiceItemAction_MouseEnter(object sender, MouseEventArgs e)
        {
            InvoiceItemAction.Opacity = 1;
            StackPanel.Opacity = 1;
            AddItemBtn.Opacity = 1;
        }

        private void InvoiceItemAction_MouseLeave(object sender, MouseEventArgs e)
        {
            InvoiceItemAction.Opacity = 0.5;
        }
    }
}
