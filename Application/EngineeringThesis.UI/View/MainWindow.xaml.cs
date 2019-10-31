using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using EngineeringThesis.Core.Models;
using EngineeringThesis.Core.Utility.ShowDialogs;
using EngineeringThesis.Core.ViewModel;
using EngineeringThesis.UI.Navigation;
using MaterialDesignThemes.Wpf;

namespace EngineeringThesis.UI.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly NavigationService _navigationService;
        public MainViewModel ViewModel;
        public MainWindow(NavigationService navigationService, MainViewModel viewModel)
        {
            InitializeComponent();

            _navigationService = navigationService;
            ViewModel = viewModel;

            InvoiceDataGrid.ItemsSource = ViewModel.GetInvoices();
            CustomerDataGrid.ItemsSource = ViewModel.GetCustomers();
        }

        private async void InvoiceDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var Invoice = InvoiceDataGrid.SelectedCells[0].Item as Invoice;
            var invoiceWindow = await _navigationService.ShowDialogAsync<InvoiceWindow>(Invoice);
        }

        private async void AddInvoiceButton_Click(object sender, RoutedEventArgs e)
        {
            var invoiceWindow = await _navigationService.ShowDialogAsync<InvoiceWindow>();
        }

        private async void AddContractorButton_Click(object sender, RoutedEventArgs e)
        {
            var addCustomerWindow = await _navigationService.ShowDialogAsync<AddCustomerWindow>();
            CustomerDataGrid.ItemsSource = ViewModel.GetCustomers();
        }

        private async void CustomerDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var customer = CustomerDataGrid.SelectedCells[0].Item as Customer;
            var editCustomerWindow = await _navigationService.ShowDialogAsync<AddCustomerWindow>(customer);
        }

        private async void DeleteInvoiceButton_Click(object sender, RoutedEventArgs e)
        {
            if (InvoiceDataGrid.SelectedCells[0].Item is Invoice invoice)
            {
                var result = await Forge.Forms.Show.Dialog("MainDialogHost").For(
                    new Warning("Czy napewno chcesz usunąć fakturę o numerze: " + invoice.InvoiceNumber,
                "Usuwanie faktury", "Tak", "Nie"));
                if (result.Action != null)
                {
                    if (result.Action.Equals("positive"))
                    {
                        ViewModel.DeleteInvoice(invoice);
                        ViewModel.Invoices.Remove(invoice);
                        InvoiceDataGrid.Items.Refresh();
                    }
                }
            }
        }

        private async void DeleteContractorButton_Click(object sender, RoutedEventArgs e)
        {
            if (CustomerDataGrid.SelectedCells[0].Item is Customer customer)
            {
                var result = await Forge.Forms.Show.Dialog("MainDialogHost").For(
                    new Warning("Czy napewno chcesz usunąć kontrahenta: " + customer.Name,
                    "Usuwanie kontrahenta", "Tak", "Nie"));
                if (result.Action != null)
                {
                    if (result.Action.Equals("positive"))
                    {
                        ViewModel.DeleteCustomer(customer);
                        ViewModel.Customers.Remove(customer);
                        CustomerDataGrid.Items.Refresh();
                    }
                }
                
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
            MaximizeWindowIcon.Kind = WindowState == WindowState.Normal ? PackIconKind.WindowMaximize : PackIconKind.WindowRestore;
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
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

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
