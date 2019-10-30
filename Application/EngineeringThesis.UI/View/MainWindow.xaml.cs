using System.Windows;
using System.Windows.Input;
using EngineeringThesis.Core.Models;
using EngineeringThesis.Core.ViewModel;
using EngineeringThesis.UI.Navigation;

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

        private void CustomerDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var customer = CustomerDataGrid.SelectedCells[0].Item as Customer;
            var editCustomerWindow = _navigationService.ShowDialogAsync<AddCustomerWindow>(customer);
            CustomerDataGrid.ItemsSource = ViewModel.GetCustomers();
        }

        private void DeleteInvoiceButton_Click(object sender, RoutedEventArgs e)
        {
            var invoice = InvoiceDataGrid.SelectedCells[0].Item as Invoice;
            ViewModel.DeleteInvoice(invoice);
            InvoiceDataGrid.ItemsSource = ViewModel.GetInvoices();
            InvoiceDataGrid.Items.Refresh();
        }

        private void DeleteContractorButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
