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

            MainViewModel.GetInvoices();
            MainViewModel.GetCustomers();

            InvoiceDataGrid.ItemsSource = MainViewModel.Invoices;
            CustomerDataGrid.ItemsSource = MainViewModel.Customers;
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
        }
    }
}
