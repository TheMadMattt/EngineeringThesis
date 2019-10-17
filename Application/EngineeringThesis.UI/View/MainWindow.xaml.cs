using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using EngineeringThesis.Core.Models;
using EngineeringThesis.UI.ViewModel;

namespace EngineeringThesis.UI.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Navigation.NavigationService _navigationService;
        public MainViewModel MainViewModel;
        public MainWindow(Navigation.NavigationService navigationService, MainViewModel mainViewModel)
        {
            InitializeComponent();

            this._navigationService = navigationService;
            MainViewModel = mainViewModel;

            MainViewModel.GetInvoices();
            MainViewModel.GetCustomers();

            InvoiceDataGrid.ItemsSource = MainViewModel.Invoices;
            CustomerDataGrid.ItemsSource = MainViewModel.Customers;
        }

        private void InvoiceDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selFaktura = InvoiceDataGrid.SelectedCells[0].Item as Invoice;
            selFaktura.Comments = "orem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.";
            InvoiceDataGrid.Items.Refresh();
        }
    }
}
