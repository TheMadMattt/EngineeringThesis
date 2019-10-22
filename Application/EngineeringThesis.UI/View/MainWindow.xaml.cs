using System.Windows;
using System.Windows.Input;
using EngineeringThesis.Core.Models;
using EngineeringThesis.UI.Navigation;
using EngineeringThesis.UI.ViewModel;

namespace EngineeringThesis.UI.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly NavigationService _navigationService;
        public MainViewModel MainViewModel;
        public MainWindow(NavigationService navigationService, MainViewModel mainViewModel)
        {
            InitializeComponent();

            _navigationService = navigationService;
            MainViewModel = mainViewModel;

            InvoiceDataGrid.ItemsSource = MainViewModel.GetInvoices();
            CustomerDataGrid.ItemsSource = MainViewModel.GetCustomers();
        }

        private async void InvoiceDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var Invoice = InvoiceDataGrid.SelectedCells[0].Item as Invoice;
            var inviceWindow = await _navigationService.ShowDialogAsync<InvoiceWindow>(Invoice);
        }

        private async void AddInvoiceButton_Click(object sender, RoutedEventArgs e)
        {
            var invoiceWindow = await _navigationService.ShowDialogAsync<InvoiceWindow>();
        }

        private async void AddContractorButton_Click(object sender, RoutedEventArgs e)
        {
            var addCustomerWindow = await _navigationService.ShowDialogAsync<AddCustomerWindow>();
            CustomerDataGrid.ItemsSource = MainViewModel.GetCustomers();
        }

        private void CustomerDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var customer = CustomerDataGrid.SelectedCells[0].Item as Customer;
            var editCustomerWindow = _navigationService.ShowDialogAsync<AddCustomerWindow>(customer);
        }
    }
}
