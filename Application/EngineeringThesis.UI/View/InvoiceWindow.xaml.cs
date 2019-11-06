using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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
            DataContext = ViewModel;
        }

        public Task ActivateAsync(object parameter)
        {
            if (parameter is Invoice invoice)
            {
                if (!string.IsNullOrEmpty(invoice.InvoiceNumber))
                {
                    ViewModel.InvoiceWithRef = invoice;
                    ViewModel.BindData(invoice);
                    ViewModel.IsUpdate = true;
                    BindInvoiceToControls();
                }
                else
                {
                    ViewModel.InvoiceWithRef = invoice;
                    ViewModel.Invoice = new Invoice();
                    ViewModel.IsUpdate = false;
                    var lastInvoice = ViewModel.GetLastInvoice();
                    ViewModel.Invoice.InvoiceNumber =
                        ViewModel.CreateInvoiceNumber(lastInvoice.InvoiceNumber);
                    ViewModel.Invoice.InvoiceItems = new List<InvoiceItem>();
                    BindNewInvoiceToControls();
                }

                SetControlsEditing();
            }
            
            return Task.CompletedTask;
        }

        private void SetControlsEditing()
        {
            if (ViewModel.IsUpdate)
            {
                AddItemBtn.Visibility = Visibility.Hidden;
                EditItemBtn.Visibility = Visibility.Hidden; 
                EditItemBtn.IsEnabled = false;
                DeleteItemBtn.Visibility = Visibility.Hidden;
                DeleteItemBtn.IsEnabled = false;
                SaveInvoiceBtn.Visibility = Visibility.Hidden;
                EditingInvoiceBtn.Visibility = Visibility.Visible;
                ContractorComboBox.IsEnabled = false;
                SellerComboBox.IsEnabled = false;
                InvoiceDatePicker.IsEnabled = false;
                PaymentTypeComboBox.IsEnabled = false;
                PaymentDeadlineDatePicker.IsEnabled = false;
                IsPaidCheckBox.IsEnabled = false;
                PaidDatePicker.IsEnabled = false;
                AddContractorBtn.IsEnabled = false;
                AddSellerBtn.IsEnabled = false;
            }
        }

        private void BindNewInvoiceToControls()
        {
            ContractorComboBox.SelectedItem = ViewModel.Contractors[0];
            SellerComboBox.SelectedItem = ViewModel.Sellers[0];
            ViewModel.Invoice.InvoiceDate = DateTime.Today;
            PaymentTypeComboBox.SelectedItem = ViewModel.PaymentTypes[0];
            ViewModel.Invoice.PaymentDeadline = DateTime.Today;
            IsPaidCheckBox.IsChecked = false;
            InvoiceItemsDataGrid.ItemsSource = ViewModel.Invoice.InvoiceItems;
            TitleLabel.Content = "Faktura " + ViewModel.Invoice.InvoiceNumber;

            ViewModel.Invoice.PaymentTypeId = ((PaymentType) PaymentTypeComboBox.SelectedItem).Id;
            ViewModel.Invoice.ContractorId = ((Customer) ContractorComboBox.SelectedItem).Id;
            ViewModel.Invoice.SellerId = ((Customer) SellerComboBox.SelectedItem).Id;
        }

        public void BindInvoiceToControls()
        {
            ContractorComboBox.SelectedItem = ViewModel.Contractors.Find(x => x.Id == ViewModel.Invoice.ContractorId);
            SellerComboBox.SelectedItem = ViewModel.Sellers.Find(x => x.Id == ViewModel.Invoice.SellerId);
            PaymentTypeComboBox.SelectedItem = ViewModel.PaymentTypes.Find(x => x.Id == ViewModel.Invoice.PaymentTypeId);
            if (ViewModel.Invoice.PaymentDate.HasValue)
            {
                IsPaidCheckBox.IsChecked = true;
            }
            TitleLabel.Content = "Faktura " + ViewModel.Invoice.InvoiceNumber;
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
            if (Utility.IsNotInvoiceItemNullOrEmpty(invoiceItem))
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

                if (Utility.IsNotInvoiceItemNullOrEmpty(invoiceItem))
                {
                    var invoiceItemsList = ViewModel.Invoice.InvoiceItems.ToList();
                    var index = invoiceItemsList.FindIndex(x => x.Id == invoiceItem.Id);
                    invoiceItemsList[index] = invoiceItem;
                }
                InvoiceItemsDataGrid.Items.Refresh();
            }
            else
            {
                await Forge.Forms.Show.Dialog("InvoiceDialogHost").For(new Information("Żaden produkt nie został wybrany", "Zaznacz produkt", "OK"));
            }
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;

        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
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
            MaximizeWindowIcon.Kind = WindowState == WindowState.Normal ? PackIconKind.WindowMaximize : PackIconKind.WindowRestore;
        }

        private void InvoiceItemAction_MouseEnter(object sender, MouseEventArgs e)
        {
            InvoiceItemAction.Opacity = 1;
            AddItemBtn.Opacity = 1;
        }

        private void InvoiceItemAction_MouseLeave(object sender, MouseEventArgs e)
        {
            InvoiceItemAction.Opacity = 0.5;
        }

        private void SaveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ViewModel.Invoice.InvoiceItems.Count > 0;
        }

        private async void SaveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (ContractorComboBox.SelectedIndex >= 0 && SellerComboBox.SelectedIndex >= 0)
            {
                if (ContractorComboBox.SelectedItem is Customer contractor)
                {
                    ViewModel.Invoice.Contractor = contractor;
                }

                if (SellerComboBox.SelectedItem is Customer seller)
                {
                    ViewModel.Invoice.Seller = seller;
                }

                if (PaymentTypeComboBox.SelectedItem is PaymentType paymentType)
                {
                    ViewModel.Invoice.PaymentType = paymentType;
                }

                ViewModel.BindDataToRef();
                ViewModel.SaveInvoice();
                Close();
            }
            else if(ContractorComboBox.SelectedIndex < 0)
            {
                await Forge.Forms.Show.Dialog("InvoiceDialogHost").For(new Information("Żaden kontrahent nie został wybrany", "Wybierz kontrahenta", "OK"));
            }else if (SellerComboBox.SelectedIndex < 0)
            {
                await Forge.Forms.Show.Dialog("InvoiceDialogHost").For(new Information("Żaden sprzedawca nie został wybrany", "Wybierz sprzedawcę", "OK"));
            }
        }

        private async void AddContractorBtn_Click(object sender, RoutedEventArgs e)
        {
            var contractor = new Utility.CustomerStruct
            {
                customer = new Customer(),
                isContractor = true
            };
            await _navigationService.ShowDialogAsync<AddCustomerWindow>(contractor);
            if (Utility.IsNotCustomerNullOrEmpty(contractor.customer))
            {
                ViewModel.Contractors.Add(contractor.customer);
                ContractorComboBox.Items.Refresh();
            }
        }

        private async void AddSellerBtn_Click(object sender, RoutedEventArgs e)
        {
            var seller = new Utility.CustomerStruct
            {
                customer = new Customer(),
                isContractor = false
            };
            await _navigationService.ShowDialogAsync<AddCustomerWindow>(seller);
            if (Utility.IsNotCustomerNullOrEmpty(seller.customer))
            {
                ViewModel.Sellers.Add(seller.customer);
                SellerComboBox.Items.Refresh();
            }
        }

        private void EditingInvoiceBtn_Click(object sender, RoutedEventArgs e)
        {
            AddItemBtn.Visibility = Visibility.Visible;
            EditItemBtn.Visibility = Visibility.Visible;
            EditItemBtn.IsEnabled = true;
            DeleteItemBtn.Visibility = Visibility.Visible;
            DeleteItemBtn.IsEnabled = true;
            SaveInvoiceBtn.Visibility = Visibility.Visible;
            EditingInvoiceBtn.Visibility = Visibility.Collapsed;
            ContractorComboBox.IsEnabled = true;
            SellerComboBox.IsEnabled = true;
            InvoiceDatePicker.IsEnabled = true;
            PaymentTypeComboBox.IsEnabled = true;
            PaymentDeadlineDatePicker.IsEnabled = true;
            IsPaidCheckBox.IsEnabled = true;
            PaidDatePicker.IsEnabled = true;
            AddContractorBtn.IsEnabled = true;
            AddSellerBtn.IsEnabled = true;
        }

        private async void InvoiceItemsDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (EditItemBtn.IsEnabled && DeleteItemBtn.IsEnabled)
            {
                if (InvoiceItemsDataGrid.SelectedItem is InvoiceItem invoiceItem)
                {
                    await _navigationService.ShowDialogAsync<InvoiceItemWindow>(invoiceItem);

                    if (Utility.IsNotInvoiceItemNullOrEmpty(invoiceItem))
                    {
                        var invoiceItemsList = ViewModel.Invoice.InvoiceItems.ToList();
                        var index = invoiceItemsList.FindIndex(x => x.Id == invoiceItem.Id);
                        invoiceItemsList[index] = invoiceItem;
                    }

                    InvoiceItemsDataGrid.Items.Refresh();
                }
                else
                {
                    await Forge.Forms.Show.Dialog("InvoiceDialogHost")
                        .For(new Information("Żaden produkt nie został wybrany", "Zaznacz produkt", "OK"));
                }
            }
        }

        private void ContractorComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ContractorComboBox.SelectedItem is Customer customer)
            {
                ViewModel.Invoice.ContractorId = customer.Id;
            }
        }

        private void SellerComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SellerComboBox.SelectedItem is Customer customer)
            {
                ViewModel.Invoice.SellerId = customer.Id;
            }
        }

        private void PaymentTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PaymentTypeComboBox.SelectedItem is PaymentType paymentType)
            {
                ViewModel.Invoice.PaymentTypeId = paymentType.Id;
            }
        }

        private void CreatePDF_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ViewModel.Invoice.InvoiceItems.Count > 0;
        }

        private void CreatePDF_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var invoice = ViewModel.GetInvoice();
            if (invoice != null)
            {
                InvoiceTemplate invoiceTemplate = new InvoiceTemplate(invoice);

                /*try
                {*/
                var pdfFile = invoiceTemplate.CreatePdf("Oryginał");

                if (pdfFile != null)
                {
                    var filePath = AppDomain.CurrentDomain.BaseDirectory + "/" + pdfFile;
                    Uri pdf = new Uri(filePath, UriKind.RelativeOrAbsolute);

                    var process = new Process
                    {
                        StartInfo = new ProcessStartInfo(@pdf.AbsolutePath)
                        {
                            CreateNoWindow = true,
                            UseShellExecute = true
                        }
                    };
                    process.Start();
                }
                /*}
                catch (Exception)
                {
                    await Forge.Forms.Show.Dialog("InvoiceDialogHost").For(new Information("Nie udało się utworzyć pliku PDF, prawdopodobnie plik PDF jest " +
                                                                                           "otwarty w innym oknie, zamknij pozostałe okna i spróbuj ponownie", "Zaznacz produkt", "OK"));
                }*/
            }
        }
    }
}
