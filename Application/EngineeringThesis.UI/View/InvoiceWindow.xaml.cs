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
                    ViewModel.Invoice.InvoiceNumber = ViewModel.CreateInvoiceNumber(lastInvoice?.InvoiceNumber);
                    CheckInvoiceNumber();
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
                SaveInvoiceBtn.Visibility = Visibility.Collapsed;
                EditingInvoiceBtn.Visibility = Visibility.Visible;
                EditingInvoiceBtn.IsEnabled = true;
                ContractorComboBox.IsEnabled = false;
                SellerComboBox.IsEnabled = false;
                InvoiceDatePicker.IsEnabled = false;
                PaymentTypeComboBox.IsEnabled = false;
                PaymentDeadlineDatePicker.IsEnabled = false;
                IsPaidCheckBox.IsEnabled = false;
                PaidDatePicker.IsEnabled = false;
                AddContractorBtn.IsEnabled = false;
                AddSellerBtn.IsEnabled = false;
                CommentsTextBox.IsEnabled = false;
                EditMenuItem.IsEnabled = false;
                DeleteMenuItem.IsEnabled = false;
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
            TitleLabel.Content = "Faktura nr " + ViewModel.Invoice.InvoiceNumber;

            ViewModel.Invoice.PaymentTypeId = ((PaymentType)PaymentTypeComboBox.SelectedItem).Id;
            ViewModel.Invoice.ContractorId = ((Customer)ContractorComboBox.SelectedItem).Id;
            ViewModel.Invoice.SellerId = ((Customer)SellerComboBox.SelectedItem).Id;

            ViewModel.SelectedContractor = ((Customer) ContractorComboBox.SelectedItem);
            ViewModel.SelectedSeller = ((Customer) SellerComboBox.SelectedItem);
        }

        public void BindInvoiceToControls()
        {
            var selectedContractor = ViewModel.Contractors.FirstOrDefault(x => x.Id == ViewModel.Invoice.ContractorId);
            ContractorComboBox.SelectedItem = selectedContractor;
            ViewModel.SelectedContractor = selectedContractor;
            var selectedSeller = ViewModel.Sellers.FirstOrDefault(x => x.Id == ViewModel.Invoice.SellerId);
            SellerComboBox.SelectedItem = selectedSeller;
            ViewModel.SelectedSeller = selectedSeller;
            PaymentTypeComboBox.SelectedItem =
                ViewModel.PaymentTypes.Find(x => x.Id == ViewModel.Invoice.PaymentTypeId);
            if (ViewModel.Invoice.PaymentDate.HasValue) IsPaidCheckBox.IsChecked = true;
            TitleLabel.Content = "Faktura " + ViewModel.Invoice.InvoiceNumber;
            InvoiceItemsDataGrid.ItemsSource = ViewModel.Invoice.InvoiceItems;
        }

        private async void AddItemBtn_Click(object sender, RoutedEventArgs e)
        {
            var invoiceItem = new InvoiceItem();
            await _navigationService.ShowDialogAsync<InvoiceItemWindow>(invoiceItem);
            if (Utility.IsNotInvoiceItemNullOrEmpty(invoiceItem))
            {
                ViewModel.Invoice.InvoiceItems.Add(invoiceItem);
                InvoiceItemsDataGrid.Items.Refresh();
            }
        }

        private void EditItemBtn_Click(object sender, RoutedEventArgs e)
        {
            EditInvoiceItem();
        }

        private void EditMenuItem_Click(object sender, RoutedEventArgs e)
        {
            EditInvoiceItem();
        }

        private void DeleteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            DeleteInvoiceItem();
        }

        private void DeleteItemBtn_Click(object sender, RoutedEventArgs e)
        {
            DeleteInvoiceItem();
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
                    WindowState = WindowState.Normal;
                else if (WindowState == WindowState.Normal) WindowState = WindowState.Maximized;
            }

            if (e.LeftButton == MouseButtonState.Pressed) DragMove();
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            MaximizeWindowIcon.Kind = WindowState == WindowState.Normal
                ? PackIconKind.WindowMaximize
                : PackIconKind.WindowRestore;
        }

        private void SaveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true; //ViewModel.Invoice.InvoiceItems.Count > 0;
        }

        private async void SaveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (ContractorComboBox.SelectedIndex >= 0 && SellerComboBox.SelectedIndex >= 0)
                try
                {
                    if (ContractorComboBox.SelectedItem is Customer contractor)
                        ViewModel.Invoice.Contractor = contractor;

                    if (SellerComboBox.SelectedItem is Customer seller) ViewModel.Invoice.Seller = seller;

                    if (PaymentTypeComboBox.SelectedItem is PaymentType paymentType)
                        ViewModel.Invoice.PaymentType = paymentType;

                    if (IsPaidCheckBox.IsChecked != null && (bool)!IsPaidCheckBox.IsChecked)
                        ViewModel.Invoice.PaymentDate = null;

                    ViewModel.BindDataToRef();
                    ViewModel.SaveInvoice();
                    ViewModel.SaveLastInvoiceNumber(ViewModel.Invoice.InvoiceNumber);
                    Close();
                }
                catch (Exception)
                {
                    await Forge.Forms.Show.Dialog("InvoiceDialogHost").For(new ErrorInfo(
                        "Wystąpił błąd przy zapisywaniu faktury. " +
                        "Sprawdź czy wszystkie dane zostały wpisane poprawnie i spróbuj ponownie",
                        "Błąd zapisywania faktury", "OK"));
                }
            else if (ContractorComboBox.SelectedIndex < 0)
                await Forge.Forms.Show.Dialog("InvoiceDialogHost")
                    .For(new Information("Żaden kontrahent nie został wybrany", "Wybierz kontrahenta", "OK"));
            else if (SellerComboBox.SelectedIndex < 0)
                await Forge.Forms.Show.Dialog("InvoiceDialogHost")
                    .For(new Information("Żaden sprzedawca nie został wybrany", "Wybierz sprzedawcę", "OK"));
        }

        private async void AddContractorBtn_Click(object sender, RoutedEventArgs e)
        {
            var contractor = new Utility.CustomerStruct
            {
                Customer = new Customer(),
                IsContractor = true
            };
            await _navigationService.ShowDialogAsync<AddCustomerWindow>(contractor);
            if (Utility.IsNotCustomerNullOrEmpty(contractor.Customer))
            {
                ViewModel.Contractors.Add(contractor.Customer);
                ContractorComboBox.Items.Refresh();
            }
        }

        private async void EditContractorBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ContractorComboBox.SelectedItem != null)
            {
                if (ContractorComboBox.SelectedItem is Customer customer)
                {
                    await _navigationService.ShowDialogAsync<AddCustomerWindow>(customer);
                    var amount = ContractorComboBox.Items.Count;
                    if (amount - 1 != ContractorComboBox.SelectedIndex && amount - 1 > 0)
                    {
                        ContractorComboBox.SelectedIndex = amount - 1;
                    }
                    else
                    {
                        ContractorComboBox.SelectedIndex = 0;
                    }
                    ContractorComboBox.SelectedItem = customer;
                    ViewModel.SelectedContractor = customer;
                }
            }
            else
            {
                await Forge.Forms.Show.Dialog("MainDialogHost")
                    .For(new Information("Żaden kontrahent nie został wybrany", "Zaznacz kontrahenta", "OK"));
            }
        }

        private async void AddSellerBtn_Click(object sender, RoutedEventArgs e)
        {
            var seller = new Utility.CustomerStruct
            {
                Customer = new Customer(),
                IsContractor = false
            };
            await _navigationService.ShowDialogAsync<AddCustomerWindow>(seller);
            if (Utility.IsNotCustomerNullOrEmpty(seller.Customer))
            {
                ViewModel.Sellers.Add(seller.Customer);
                SellerComboBox.Items.Refresh();
            }
        }
        private async void EditSellerBtn_Click(object sender, RoutedEventArgs e)
        {
            if (SellerComboBox.SelectedItem != null)
            {
                if (SellerComboBox.SelectedItem is Customer customer)
                {
                    await _navigationService.ShowDialogAsync<AddCustomerWindow>(customer);
                    var amount = SellerComboBox.Items.Count;
                    if (amount - 1 != SellerComboBox.SelectedIndex && amount - 1 > 0)
                    {
                        SellerComboBox.SelectedIndex = amount - 1;
                    }
                    else
                    {
                        SellerComboBox.SelectedIndex = 0;
                    }
                    SellerComboBox.SelectedItem = customer;
                    ViewModel.SelectedSeller = customer;
                }
            }
            else
            {
                await Forge.Forms.Show.Dialog("MainDialogHost")
                    .For(new Information("Żaden sprzedawca nie został wybrany", "Zaznacz sprzedawcę", "OK"));
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
            EditingInvoiceBtn.IsEnabled = false;
            ContractorComboBox.IsEnabled = true;
            SellerComboBox.IsEnabled = true;
            InvoiceDatePicker.IsEnabled = true;
            PaymentTypeComboBox.IsEnabled = true;
            PaymentDeadlineDatePicker.IsEnabled = true;
            IsPaidCheckBox.IsEnabled = true;
            PaidDatePicker.IsEnabled = IsPaidCheckBox.IsChecked != null && (bool)IsPaidCheckBox.IsChecked;
            AddContractorBtn.IsEnabled = true;
            AddSellerBtn.IsEnabled = true;
            CommentsTextBox.IsEnabled = true;
            EditMenuItem.IsEnabled = true;
            DeleteMenuItem.IsEnabled = true;
        }

        private void ContractorComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ContractorComboBox.SelectedItem is Customer customer)
            {
                ViewModel.Invoice.ContractorId = customer.Id;
                ViewModel.Invoice.Contractor = customer;
                ViewModel.SelectedContractor = customer;
            }
        }

        private void SellerComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SellerComboBox.SelectedItem is Customer customer)
            {
                ViewModel.Invoice.SellerId = customer.Id;
                ViewModel.Invoice.Seller = customer;
                ViewModel.SelectedSeller = customer;
            }
        }

        private void PaymentTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PaymentTypeComboBox.SelectedItem is PaymentType paymentType)
                ViewModel.Invoice.PaymentTypeId = paymentType.Id;
        }

        private void CreatePDF_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ViewModel.Invoice.InvoiceItems.Count > 0 
                           && ContractorComboBox.SelectedItem != null 
                           && SellerComboBox.SelectedItem != null;
        }

        private async void CreatePDF_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var invoice = ViewModel.GetInvoice();
            if (invoice != null)
            {
                var invoiceTemplate = new InvoiceTemplate(invoice);

                try
                {
                    Utility.InvoiceTemplateStruct templateOriginal = new Utility.InvoiceTemplateStruct
                    {
                        CommentsCheckBox = CommentsCheckBox.IsChecked == true,
                        InvoicePersonCheckBox = InvoicePersonCheckBox.IsChecked == true,
                        PickupPersonCheckBox = PickupPersonCheckBox.IsChecked == true
                    };
                    Utility.InvoiceTemplateStruct templateCopy = new Utility.InvoiceTemplateStruct
                    {
                        CommentsCheckBox = CommentsCheckBox.IsChecked == true,
                        InvoicePersonCheckBox = InvoicePersonCheckBox.IsChecked == true,
                        PickupPersonCheckBox = PickupPersonCheckBox.IsChecked == true
                    };

                    if (IsProformaCheckBox.IsChecked == true)
                    {
                        templateOriginal.InvoiceType = Utility.InvoiceTypeTemplateEnum.Original;
                        templateOriginal.InvoiceTitle = Utility.InvoiceTypeTemplateEnum.Proforma;

                        templateCopy.InvoiceType = Utility.InvoiceTypeTemplateEnum.Copy;
                        templateCopy.InvoiceTitle = Utility.InvoiceTypeTemplateEnum.Proforma;
                    }
                    else
                    {
                        templateOriginal.InvoiceType = Utility.InvoiceTypeTemplateEnum.Original;
                        templateOriginal.InvoiceTitle = Utility.InvoiceTypeTemplateEnum.Original;

                        templateCopy.InvoiceType = Utility.InvoiceTypeTemplateEnum.Copy;
                        templateCopy.InvoiceTitle = Utility.InvoiceTypeTemplateEnum.Original;
                    }

                    var pdfFile = invoiceTemplate.CreatePdf(templateOriginal);
                    invoiceTemplate.CreatePdf(templateCopy);


                    if (pdfFile != null)
                    {
                        var filePath = AppDomain.CurrentDomain.BaseDirectory + pdfFile;
                        var pdf = new Uri(filePath, UriKind.RelativeOrAbsolute);

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

                }
                catch (Exception)
                {
                    await Forge.Forms.Show.Dialog("InvoiceDialogHost").For(
                        new Information("Nie udało się utworzyć pliku PDF, " +
                                        "prawdopodobnie plik PDF jest otwarty w innym oknie, " +
                                        "zamknij pozostałe okna i spróbuj ponownie", "Zaznacz produkt", "OK"));
                }
            }
        }

        private void IsPaidCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            PaidDatePickerEnable(sender);
        }

        private void IsPaidCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            PaidDatePickerEnable(sender);
        }

        private void PaidDatePickerEnable(object sender)
        {
            var isChecked = ((CheckBox)sender).IsChecked;
            if (isChecked != null)
                PaidDatePicker.IsEnabled = (bool)isChecked;
        }

        private async void PaymentDeadlineDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ViewModel != null && !EditingInvoiceBtn.IsEnabled)
            {
                var comparePaymentDate = ViewModel.Invoice.PaymentDeadline.CompareTo(ViewModel.Invoice?.PaymentDate);

                InvoiceDialogHost.IsOpen = false;
                if (comparePaymentDate < 0)
                {
                    await Forge.Forms.Show.Dialog("InvoiceDialogHost").For(
                        new Information("Data płatności nie może być późniejsza niż termin płatności",
                            "Wprowadź prawidłową datę",
                            "OK"));
                    PaidDatePicker.SelectedDate = null;
                }

                var compareInvoiceDate = ViewModel.Invoice.PaymentDeadline.CompareTo(ViewModel.Invoice.InvoiceDate);

                if (compareInvoiceDate < 0)
                {
                    await Forge.Forms.Show.Dialog("InvoiceDialogHost").For(
                        new Information("Termin płatności nie może być wcześniejszy niż data wystawienia faktury",
                            "Wprowadź prawidłową datę",
                            "OK"));
                    if (PaymentDeadlineDatePicker.SelectedDate != null) InvoiceDatePicker.SelectedDate = null;
                }

                InvoiceDialogHost.IsOpen = false;
            }
        }

        private async void InvoiceDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            CheckInvoiceNumber();
            if (ViewModel != null && !EditingInvoiceBtn.IsEnabled)
            {
                var comparePaymentDeadline = ViewModel.Invoice.InvoiceDate.CompareTo(ViewModel.Invoice.PaymentDeadline);
                var comparePaymentDate = ViewModel.Invoice.InvoiceDate.CompareTo(ViewModel.Invoice?.PaymentDate);

                InvoiceDialogHost.IsOpen = false;
                if (comparePaymentDeadline > 0)
                {
                    await Forge.Forms.Show.Dialog("InvoiceDialogHost").For(
                        new Information("Termin płatności nie może być wcześniejszy niż data wystawienia faktury",
                            "Wprowadź prawidłową datę",
                            "OK"));
                    if (InvoiceDatePicker.SelectedDate != null) PaymentDeadlineDatePicker.SelectedDate = null;
                }

                if (comparePaymentDate > 0 && ViewModel.Invoice.PaymentDate != null)
                {
                    await Forge.Forms.Show.Dialog("InvoiceDialogHost").For(
                        new Information("Data płatności nie może być wcześniejsza niż data wystawienia faktury",
                            "Wprowadź prawidłową datę",
                            "OK"));
                    PaidDatePicker.SelectedDate = null;
                }

                InvoiceDialogHost.IsOpen = false;
            }
        }

        private async void PaidDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ViewModel != null && !EditingInvoiceBtn.IsEnabled)
            {
                var comparePaymentDeadline =
                    ViewModel.Invoice.PaymentDate?.CompareTo(ViewModel.Invoice.PaymentDeadline);
                var compareInvoiceDate = ViewModel.Invoice.PaymentDate?.CompareTo(ViewModel.Invoice.InvoiceDate);

                InvoiceDialogHost.IsOpen = false;
                if (comparePaymentDeadline > 0)
                {
                    await Forge.Forms.Show.Dialog("InvoiceDialogHost").For(
                        new Information("Data płatności nie może być późniejsza niż termin płatności",
                            "Wprowadź prawidłową datę",
                            "OK"));
                    PaidDatePicker.SelectedDate = null;
                }

                if (compareInvoiceDate < 0)
                {
                    await Forge.Forms.Show.Dialog("InvoiceDialogHost").For(
                        new Information("Data płatności nie może być wcześniejsza niż data wystawienia",
                            "Wprowadź prawidłową datę",
                            "OK"));
                    PaidDatePicker.SelectedDate = null;
                }

                InvoiceDialogHost.IsOpen = false;
            }
        }

        private async void CreateCorrectionPDF_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var invoice = ViewModel.GetInvoice();
            if (invoice != null)
            {
                var invoiceTemplate = new InvoiceTemplate(invoice);

                try
                {
                    Utility.InvoiceTemplateStruct templateOriginal = new Utility.InvoiceTemplateStruct
                    {
                        CommentsCheckBox = CommentsCheckBox.IsChecked == true,
                        InvoicePersonCheckBox = InvoicePersonCheckBox.IsChecked == true,
                        PickupPersonCheckBox = PickupPersonCheckBox.IsChecked == true,
                        InvoiceTitle = Utility.InvoiceTypeTemplateEnum.Correction,
                        InvoiceType = Utility.InvoiceTypeTemplateEnum.Original
                    };
                    Utility.InvoiceTemplateStruct templateCopy = new Utility.InvoiceTemplateStruct
                    {
                        CommentsCheckBox = CommentsCheckBox.IsChecked == true,
                        InvoicePersonCheckBox = InvoicePersonCheckBox.IsChecked == true,
                        PickupPersonCheckBox = PickupPersonCheckBox.IsChecked == true,
                        InvoiceTitle = Utility.InvoiceTypeTemplateEnum.Correction,
                        InvoiceType = Utility.InvoiceTypeTemplateEnum.Copy
                    };

                    var pdfFile = invoiceTemplate.CreatePdf(templateOriginal);
                    invoiceTemplate.CreatePdf(templateCopy);

                    if (pdfFile != null)
                    {
                        var filePath = AppDomain.CurrentDomain.BaseDirectory + pdfFile;
                        var pdf = new Uri(filePath, UriKind.RelativeOrAbsolute);

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

                }
                catch (Exception)
                {
                    await Forge.Forms.Show.Dialog("InvoiceDialogHost").For(
                        new Information("Nie udało się utworzyć pliku PDF, " +
                                        "prawdopodobnie plik PDF jest otwarty w innym oknie, " +
                                        "zamknij pozostałe okna i spróbuj ponownie", "Zaznacz produkt", "OK"));
                }
            }
        }

        private void CheckInvoiceNumber()
        {
            if (InvoiceDatePicker.SelectedDate != null && ViewModel != null && !ViewModel.IsUpdate)
            {
                if (ViewModel.InvoiceYear != InvoiceDatePicker.SelectedDate.Value.Year)
                {
                    var lastInvoiceNumber = ViewModel.GetLastInvoiceNumber(InvoiceDatePicker.SelectedDate.Value.Year);

                    if (lastInvoiceNumber != null)
                    {
                        ViewModel.Invoice.InvoiceNumber =
                            ViewModel.CreateInvoiceNumber(lastInvoiceNumber.InvoiceNumber);
                        TitleLabel.Content = "Faktura nr " + ViewModel.Invoice.InvoiceNumber;
                    }
                    else
                    {
                        ViewModel.InvoiceYear = InvoiceDatePicker.SelectedDate.Value.Year;
                        ViewModel.InvoiceNumber = 1;
                        ViewModel.Invoice.InvoiceNumber = ViewModel.InvoiceNumber + "/" + ViewModel.InvoiceYear;
                        TitleLabel.Content = "Faktura nr " + ViewModel.Invoice.InvoiceNumber;
                    }
                }
            }
        }

        private async void EditInvoiceItem()
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

        private async void DeleteInvoiceItem()
        {
            if (InvoiceItemsDataGrid.SelectedItem != null)
            {
                var result = await Forge.Forms.Show.Dialog("InvoiceDialogHost").For(new Warning(
                    "Czy napewno chcesz usunąć: " + ((InvoiceItem)InvoiceItemsDataGrid.SelectedItem).Name,
                    "Usuwanie produktu", "Tak", "Nie"));
                if (result.Action != null)
                    if (result.Action.Equals("positive"))
                    {
                        ViewModel.Invoice.InvoiceItems.Remove((InvoiceItem)InvoiceItemsDataGrid.SelectedItem);
                        InvoiceItemsDataGrid.Items.Refresh();
                    }
            }
            else
            {
                await Forge.Forms.Show.Dialog("InvoiceDialogHost")
                    .For(new Information("Żaden produkt nie został wybrany", "Zaznacz produkt", "OK"));
            }
        }
    }
}