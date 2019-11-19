using System.Linq;
using System.Windows;
using System.Windows.Input;
using EngineeringThesis.Core.Models;
using EngineeringThesis.Core.Utility;
using EngineeringThesis.Core.Utility.ShowDialogs;
using EngineeringThesis.Core.ViewModel;
using EngineeringThesis.UI.Navigation;
using MaterialDesignThemes.Wpf;

namespace EngineeringThesis.UI.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly NavigationService _navigationService;
        public MainViewModel ViewModel;
        public MainWindow(NavigationService navigationService, MainViewModel viewModel)
        {
            InitializeComponent();

            _navigationService = navigationService;
            ViewModel = viewModel;

            InvoiceDataGrid.ItemsSource = ViewModel.GetInvoices();
            ContractorDataGrid.ItemsSource = ViewModel.GetContractors();
            SellerDataGrid.ItemsSource = ViewModel.GetSellers();
        }

        private async void AddContractorButton_Click(object sender, RoutedEventArgs e)
        {
            var customer = new Customer();
            await _navigationService.ShowDialogAsync<AddCustomerWindow>(customer);
            if (Utility.IsNotCustomerNullOrEmpty(customer))
            {
                AddCustomer(customer);
            }
        }

        private void EditContractorButton_Click(object sender, RoutedEventArgs e)
        { 
            EditContractor();   
        }

        private void EditContractorMenu_Click(object sender, RoutedEventArgs e)
        {
            EditContractor();
        }

        private void DeleteContractorMenu_Click(object sender, RoutedEventArgs e)
        {
            DeleteContractor();
        }

        private void DeleteContractorButton_Click(object sender, RoutedEventArgs e)
        {
            DeleteContractor();
        }

        private async void AddSellerButton_Click(object sender, RoutedEventArgs e)
        {
            var customer = new Customer();
            await _navigationService.ShowDialogAsync<AddCustomerWindow>(customer);
            if (Utility.IsNotCustomerNullOrEmpty(customer))
            {
                AddCustomer(customer);
            }
        }

        private void EditSellerButton_Click(object sender, RoutedEventArgs e)
        {
            EditSeller();
        }

        private void EditSellerMenu_Click(object sender, RoutedEventArgs e)
        {
            EditSeller();
        }

        private void DeleteSellerMenu_Click(object sender, RoutedEventArgs e)
        {
            DeleteSeller();
        }

        private void DeleteSellerButton_Click(object sender, RoutedEventArgs e)
        {
            DeleteSeller();
        }

        private async void AddInvoiceButton_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.Contractors.Count > 0 && ViewModel.Sellers.Count > 0)
            {
                var invoice = new Invoice();
                await _navigationService.ShowDialogAsync<InvoiceWindow>(invoice);
                if (Utility.IsNotInvoiceNullOrEmpty(invoice))
                {
                    ViewModel.Invoices.Add(invoice);
                    InvoiceDataGrid.Items.Refresh();
                }
                SellerDataGrid.Items.Refresh();
                ContractorDataGrid.Items.Refresh();
            }
            else
            {
                await Forge.Forms.Show.Dialog("MainDialogHost")
                    .For(new Information("Dodaj kontrahentów i sprzedawców, a następnie utwórz nową fakturę", "Brak kontrahentów i sprzedawców", "OK"));
            }
        }

        private void EditInvoiceButton_Click(object sender, RoutedEventArgs e)
        {
            EditInvoice();
        }

        private void EditInvoiceMenu_Click(object sender, RoutedEventArgs e)
        {
            EditInvoice();
        }

        private void DeleteInvoiceButton_Click(object sender, RoutedEventArgs e)
        {
            DeleteInvoice();
        }

        private void DeleteInvoiceMenu_Click(object sender, RoutedEventArgs e)
        {
            DeleteInvoice();
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

        private void AddCustomer(Customer customer)
        {
            if (customer.CustomerType.Id == 1)
            {
                ViewModel.Contractors.Add(customer);
                ContractorDataGrid.Items.Refresh();
            }
            else
            {
                ViewModel.Sellers.Add(customer);
                SellerDataGrid.Items.Refresh();
            }
        }

        private void SearchContractorTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (!string.IsNullOrEmpty(SearchContractorTextBox.Text))
            {
                var filtered =
                    ViewModel.Contractors.Where(customer =>
                        customer.Name.ToLower().Contains(SearchContractorTextBox.Text.ToLower()) ||
                        customer.NIP.Contains(SearchContractorTextBox.Text.ToLower()) ||
                        customer.REGON.Contains(SearchContractorTextBox.Text.ToLower()));

                ContractorDataGrid.ItemsSource = filtered;
            }
            else
            {
                ContractorDataGrid.ItemsSource = ViewModel.Contractors;
            }
        }

        private void SearchSellerTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (!string.IsNullOrEmpty(SearchSellerTextBox.Text))
            {
                var filtered =
                    ViewModel.Sellers.Where(customer =>
                        customer.Name.ToLower().Contains(SearchSellerTextBox.Text.ToLower()) ||
                        customer.NIP.Contains(SearchSellerTextBox.Text.ToLower()) ||
                        customer.REGON.Contains(SearchSellerTextBox.Text.ToLower()));

                SellerDataGrid.ItemsSource = filtered;
            }
            else
            {
                SellerDataGrid.ItemsSource = ViewModel.Sellers;
            }
        }

        private void SearchInvoiceTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (!string.IsNullOrEmpty(SearchInvoiceTextBox.Text))
            {
                var filtered =
                    ViewModel.Invoices.Where(invoice =>
                        invoice.InvoiceNumber.Contains(SearchInvoiceTextBox.Text.ToLower()) ||
                        invoice.Seller.Name.ToLower().Contains(SearchInvoiceTextBox.Text.ToLower()) ||
                        invoice.Contractor.Name.ToLower().Contains(SearchInvoiceTextBox.Text.ToLower()));

                InvoiceDataGrid.ItemsSource = filtered;
            }
            else
            {
                InvoiceDataGrid.ItemsSource = ViewModel.Invoices;
            }
        }

        private async void DeleteInvoice()
        {
            if (InvoiceDataGrid.SelectedCells.Count > 0)
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
            else
            {
                await Forge.Forms.Show.Dialog("MainDialogHost").For(new Information("Żadna faktura nie została wybrana", "Zaznacz fakturę", "OK"));
            }
        }

        private async void EditInvoice()
        {
            if (InvoiceDataGrid.SelectedCells.Count > 0)
            {
                if (InvoiceDataGrid.SelectedCells[0].Item is Invoice invoice)
                {
                    await _navigationService.ShowDialogAsync<InvoiceWindow>(invoice);
                    if (Utility.IsNotInvoiceNullOrEmpty(invoice))
                    {
                        var index = ViewModel.Invoices.FindIndex(x => x.Id == invoice.Id);
                        ViewModel.Invoices[index] = invoice;
                        InvoiceDataGrid.Items.Refresh();
                    }
                    SellerDataGrid.Items.Refresh();
                    ContractorDataGrid.Items.Refresh();
                }
            }
            else
            {
                await Forge.Forms.Show.Dialog("MainDialogHost")
                    .For(new Information("Żadna faktura nie została wybrana", "Zaznacz fakturę", "OK"));
            }
        }

        private async void EditContractor()
        {
            if (ContractorDataGrid.SelectedCells.Count > 0)
            {
                if (ContractorDataGrid.SelectedCells[0].Item is Customer customer)
                {
                    await _navigationService.ShowDialogAsync<AddCustomerWindow>(customer);
                    ContractorDataGrid.Items.Refresh();
                }
            }
            else
            {
                await Forge.Forms.Show.Dialog("MainDialogHost")
                    .For(new Information("Żaden kontrahent nie został wybrany", "Zaznacz kontrahenta", "OK"));
            }
        }

        private async void DeleteContractor()
        {
            if (ContractorDataGrid.SelectedCells.Count > 0)
            {
                if (ContractorDataGrid.SelectedCells[0].Item is Customer customer)
                {
                    var result = await Forge.Forms.Show.Dialog("MainDialogHost").For(
                        new Warning("Czy napewno chcesz usunąć kontrahenta: " + customer.Name,
                            "Usuwanie kontrahenta", "Tak", "Nie"));
                    if (result.Action != null)
                    {
                        if (result.Action.Equals("positive"))
                        {
                            ViewModel.DeleteCustomer(customer);
                            ViewModel.Contractors.Remove(customer);
                            ContractorDataGrid.Items.Refresh();
                        }
                    }

                }
            }
            else
            {
                await Forge.Forms.Show.Dialog("MainDialogHost")
                    .For(new Information("Żaden kontrahent nie został wybrany", "Zaznacz kontrahenta", "OK"));
            }
        }

        private async void EditSeller()
        {
            if (SellerDataGrid.SelectedCells.Count > 0)
            {
                if (SellerDataGrid.SelectedCells[0].Item is Customer customer)
                {
                    await _navigationService.ShowDialogAsync<AddCustomerWindow>(customer);
                    SellerDataGrid.Items.Refresh();
                }
            }
            else
            {
                await Forge.Forms.Show.Dialog("MainDialogHost")
                    .For(new Information("Żaden sprzedawca nie został wybrany", "Zaznacz sprzedawcę", "OK"));
            }
        }

        private async void DeleteSeller()
        {
            if (SellerDataGrid.SelectedCells.Count > 0)
            {
                if (SellerDataGrid.SelectedCells[0].Item is Customer customer)
                {
                    var result = await Forge.Forms.Show.Dialog("MainDialogHost").For(
                        new Warning("Czy napewno chcesz usunąć kontrahenta: " + customer.Name,
                            "Usuwanie kontrahenta", "Tak", "Nie"));
                    if (result.Action != null)
                    {
                        if (result.Action.Equals("positive"))
                        {
                            ViewModel.DeleteCustomer(customer);
                            ViewModel.Sellers.Remove(customer);
                            SellerDataGrid.Items.Refresh();
                        }
                    }
                }
            }
            else
            {
                await Forge.Forms.Show.Dialog("MainDialogHost")
                    .For(new Information("Żaden sprzedawca nie został wybrany", "Zaznacz sprzedawcę", "OK"));
            }
        }
    }
}
