using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using EngineeringThesis.Core.Models;
using EngineeringThesis.Core.Utility.ShowDialogs;
using EngineeringThesis.Core.ViewModel;
using EngineeringThesis.UI.Navigation;
using MaterialDesignThemes.Wpf;

namespace EngineeringThesis.UI.View
{
    /// <summary>
    /// Interaction logic for InvoiceWindow.xaml
    /// </summary>
    public partial class InvoiceWindow : IActivable
    {
        private readonly NavigationService _navigationService;
        public InvoiceViewModel ViewModel;
        public InvoiceWindow(NavigationService navigationService, InvoiceViewModel viewModel)
        {
            InitializeComponent();
            InvoiceDatePicker.SelectedDate = DateTime.Today;
            ViewModel = viewModel;
            _navigationService = navigationService;

            ContractorComboBox.ItemsSource = ViewModel.GetContractors();
            SellerComboBox.ItemsSource = ViewModel.GetSellers();
            PaymentTypeComboBox.ItemsSource = ViewModel.GetPaymentTypes();
        }

        public Task ActivateAsync(object parameter)
        {
            if (parameter is Invoice invoice)
            {
                ViewModel.Invoice = ViewModel.GetInvoice(invoice.Id);
                BindInvoiceToControls();
            }
            else
            {
                ViewModel.Invoice = new Invoice();
                var lastInvoice = ViewModel.GetLastInvoice();
                ViewModel.Invoice.InvoiceNumber =
                    ViewModel.CreateInvoiceNumber(lastInvoice.InvoiceNumber);
                ViewModel.Invoice.InvoiceItems = new List<InvoiceItem>();
                BindNewInvoiceToControls();
            }
            

            return Task.CompletedTask;
        }

        private void BindNewInvoiceToControls()
        {
            ContractorComboBox.SelectedItem = ViewModel.Contractors[0];
            SellerComboBox.SelectedItem = ViewModel.Sellers[0];
            InvoiceDatePicker.SelectedDate = DateTime.Today;
            PaymentTypeComboBox.SelectedItem = ViewModel.PaymentTypes[0];
            PaymentDeadlineDatePicker.SelectedDate = DateTime.Today;
            IsPaidCheckBox.IsChecked = false;
            InvoiceItemsDataGrid.ItemsSource = ViewModel.Invoice.InvoiceItems;
        }

        public void BindInvoiceToControls()
        {
            ContractorComboBox.SelectedItem = ViewModel.Contractors.Find(x => x.Id == ViewModel.Invoice.ContractorId);
            SellerComboBox.SelectedItem = ViewModel.Sellers.Find(x => x.Id == ViewModel.Invoice.SellerId);
            InvoiceDatePicker.SelectedDate = ViewModel.Invoice.InvoiceDate;
            PaymentTypeComboBox.SelectedItem = ViewModel.PaymentTypes.Find(x => x.Id == ViewModel.Invoice.PaymentTypeId);
            PaymentDeadlineDatePicker.SelectedDate = ViewModel.Invoice.PaymentDeadline;
            if (ViewModel.Invoice.PaymentDate.HasValue)
            {
                IsPaidCheckBox.IsChecked = true;
                PaidDatePicker.SelectedDate = ViewModel.Invoice.PaymentDate;
            }

            InvoiceItemsDataGrid.ItemsSource = ViewModel.Invoice.InvoiceItems;
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
                        ViewModel.Invoice.InvoiceItems.Remove((InvoiceItem)InvoiceItemsDataGrid.SelectedItem);
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
            await _navigationService.ShowDialogAsync<InvoiceItemWindow>(invoiceItem);
            if (InvoiceViewModel.IsNullOrEmpty(invoiceItem))
            {
                ViewModel.Invoice.InvoiceItems.Add(invoiceItem);
                InvoiceItemsDataGrid.Items.Refresh();
            }
            
        }

        private async void EditItemBtn_Click(object sender, RoutedEventArgs e)
        {
            if (InvoiceItemsDataGrid.SelectedItem is InvoiceItem invoiceItem)
            {
                await _navigationService.ShowDialogAsync<InvoiceItemWindow>(invoiceItem);

                if (InvoiceViewModel.IsNullOrEmpty(invoiceItem))
                {
                    var invoiceItemsList = ViewModel.Invoice.InvoiceItems.ToList();
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

        private void MaximizeButton_Click(object sender, RoutedEventArgs e)
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
            if (e.ClickCount == 2)
            {
                if (WindowState == WindowState.Maximized)
                {
                    WindowState = WindowState.Normal;
                }
                else if (WindowState == WindowState.Normal)
                {
                    WindowState = WindowState.Maximized;
                }
            }
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
