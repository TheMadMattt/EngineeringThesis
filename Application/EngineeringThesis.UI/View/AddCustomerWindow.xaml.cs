using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using EngineeringThesis.Core.Models;
using EngineeringThesis.Core.Models.DisplayModels;
using EngineeringThesis.Core.Utility;
using EngineeringThesis.Core.Utility.ShowDialogs;
using EngineeringThesis.Core.ViewModel;
using EngineeringThesis.UI.Navigation;
using MaterialDesignThemes.Wpf;
using Xceed.Wpf.Toolkit;
using WindowState = System.Windows.WindowState;

namespace EngineeringThesis.UI.View
{
    /// <summary>
    /// Interaction logic for AddCustomerWindow.xaml
    /// </summary>
    public partial class AddCustomerWindow : IActivable
    {
        public AddCustomerViewModel ViewModel;

        public AddCustomerWindow(AddCustomerViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;

            CustomerTypeComboBox.ItemsSource = ViewModel.GetCustomerTypes();
            DataContext = ViewModel;
        }

        public Task ActivateAsync(object parameter)
        {
            if (parameter is Customer customer)
            {
                ViewModel.CustomerWithRef = customer;
                if (!string.IsNullOrEmpty(customer.Name))
                {
                    ViewModel.BindData(customer);
                    ViewModel.SplitAddress(customer.StreetNumber);
                    ViewModel.IsUpdate = true;
                    PrepareControls();
                }
                else
                {
                    ViewModel.IsUpdate = false;
                    ViewModel.Customer = new CustomerDisplayModel();
                }
            }
            else if (parameter is Utility.CustomerStruct customerStruct)
            {
                ViewModel.CustomerWithRef = customerStruct.Customer;
                if (!string.IsNullOrEmpty(customerStruct.Customer.Name))
                {
                    ViewModel.BindData(customerStruct.Customer);
                    ViewModel.SplitAddress(customerStruct.Customer.StreetNumber);
                    ViewModel.IsUpdate = true;
                    PrepareControls();
                }
                else
                {
                    ViewModel.Customer = new CustomerDisplayModel();
                    CustomerTypeComboBox.IsEnabled = false;
                    CustomerTypeComboBox.SelectedItem = customerStruct.IsContractor ? 
                        ViewModel.CustomerTypes.Find(x => x.Id == 1) : ViewModel.CustomerTypes.Find(x => x.Id == 2);
                }
            }

            SetControlsEnabled();

            return Task.CompletedTask;
        }

        private void SetControlsEnabled()
        {
            if (ViewModel.IsUpdate)
            {
                EditButton.Visibility = Visibility.Visible;
                ButtonsGrid.Visibility = Visibility.Collapsed;
                CustomerNameTextBox.IsEnabled = false;
                CustomerTypeComboBox.IsEnabled = false;
                CityTextBox.IsEnabled = false;
                ZipCodeTextBox.IsEnabled = false;
                StreetTextBox.IsEnabled = false;
                StreetNumberTextBox.IsEnabled = false;
                FlatNumberTextBox.IsEnabled = false;
                PhoneNumberTextBox.IsEnabled = false;
                NIPTextBox.IsEnabled = false;
                NoNIPCheckBox.IsEnabled = false;
                REGONTextBox.IsEnabled = false;
                NoREGONCheckBox.IsEnabled = false;
                BankAccountNumberTextBox.IsEnabled = false;
                NoBankAccountCheckBox.IsEnabled = false;
                CommentsTextBox.IsEnabled = false;
            }
        }

        private void PrepareControls()
        {
            NoNIPCheckBox.IsChecked = ViewModel.Customer.NIP == null;
            NoREGONCheckBox.IsChecked = ViewModel.Customer.REGON == null;
            NoBankAccountCheckBox.IsChecked = ViewModel.Customer.BankAccountNumber == null;
            CustomerTypeComboBox.SelectedItem = ViewModel.Customer.CustomerTypeId > 0 ? 
                ViewModel.CustomerTypes.Find(x => x.Id == ViewModel.Customer.CustomerTypeId) : ViewModel.CustomerTypes[0];
            if (ViewModel.IsUpdate)
            {
                CustomerTypeComboBox.IsEnabled = false;
            }
        }

        private async void AddCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            ForceValidation();
            if (ControlsHasNoError())
            {
                if (NoNIPCheckBox.IsChecked == true)
                {
                    ViewModel.Customer.NIP = null;
                }

                if (NoREGONCheckBox.IsChecked == true)
                {
                    ViewModel.Customer.REGON = null;
                }

                if (NoBankAccountCheckBox.IsChecked == true)
                {
                    ViewModel.Customer.BankAccountNumber = null;
                }

                ViewModel.BindToRefObject();
                ViewModel.CustomerWithRef.CustomerTypeId = ((CustomerType) CustomerTypeComboBox.SelectedItem).Id;
                ViewModel.CustomerWithRef.CustomerType = ((CustomerType) CustomerTypeComboBox.SelectedItem);
                ViewModel.SaveCustomer();

                Close();
            }
            else
            {
                await Forge.Forms.Show.Dialog("AddCustomerDialogHost").For(
                    new Information("Nie wszystkie pola zostały uzupełnione", "Uzupełnij pozostałe pola", "OK"));
            }

        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ForceValidation()
        {
            if (NoNIPCheckBox.IsChecked == false)
            {
                NIPTextBox.GetBindingExpression(TextBox.TextProperty)?.UpdateSource();
            }

            if (NoREGONCheckBox.IsChecked == false)
            {
                REGONTextBox.GetBindingExpression(TextBox.TextProperty)?.UpdateSource();
            }

            if (NoBankAccountCheckBox.IsChecked == false)
            {
                BankAccountNumberTextBox.GetBindingExpression(TextBox.TextProperty)?.UpdateSource();
            }

            CustomerNameTextBox.GetBindingExpression(TextBox.TextProperty)?.UpdateSource();
            CityTextBox.GetBindingExpression(TextBox.TextProperty)?.UpdateSource();
            ZipCodeTextBox.GetBindingExpression(TextBox.TextProperty)?.UpdateSource();
            FlatNumberTextBox.GetBindingExpression(TextBox.TextProperty)?.UpdateSource();
            StreetNumberTextBox.GetBindingExpression(TextBox.TextProperty)?.UpdateSource();
            StreetTextBox.GetBindingExpression(TextBox.TextProperty)?.UpdateSource();
            PhoneNumberTextBox.GetBindingExpression(TextBox.TextProperty)?.UpdateSource();
        }

        private bool ControlsHasNoError()
        {
            return !Validation.GetHasError(CustomerNameTextBox) &&
                   !Validation.GetHasError(CityTextBox) &&
                   !Validation.GetHasError(ZipCodeTextBox) && 
                   !Validation.GetHasError(FlatNumberTextBox) &&
                   !Validation.GetHasError(StreetNumberTextBox) &&
                   !Validation.GetHasError(StreetTextBox) &&
                   !Validation.GetHasError(PhoneNumberTextBox) &&
                   !Validation.GetHasError(NIPTextBox) && 
                   !Validation.GetHasError(REGONTextBox) &&
                   !Validation.GetHasError(BankAccountNumberTextBox);
        }

        private void NoNIPCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (NoNIPCheckBox.IsChecked == true)
            {
                var binding = NIPTextBox?.GetBindingExpression(TextBox.TextProperty);
                Validation.ClearInvalid(binding);
            }
            EnableControls(sender, NIPTextBox);
        }

        private void NoREGONCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (NoREGONCheckBox.IsChecked == true)
            {
                var binding = REGONTextBox?.GetBindingExpression(TextBox.TextProperty);
                Validation.ClearInvalid(binding);
            }
            EnableControls(sender, REGONTextBox);
        }

        private void NoBankAccountCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (NoBankAccountCheckBox.IsChecked == true)
            {
                var binding = BankAccountNumberTextBox?.GetBindingExpression(TextBox.TextProperty);
                Validation.ClearInvalid(binding);
            }
            EnableControls(sender, BankAccountNumberTextBox);
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

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            MaximizeWindowIcon.Kind = WindowState == WindowState.Normal ? PackIconKind.WindowMaximize : PackIconKind.WindowRestore;
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            EditButton.Visibility = Visibility.Collapsed;
            ButtonsGrid.Visibility = Visibility.Visible;
            CustomerNameTextBox.IsEnabled = true;
            CityTextBox.IsEnabled = true;
            ZipCodeTextBox.IsEnabled = true;
            StreetTextBox.IsEnabled = true;
            StreetNumberTextBox.IsEnabled = true;
            FlatNumberTextBox.IsEnabled = true;
            PhoneNumberTextBox.IsEnabled = true;
            if (NoNIPCheckBox.IsChecked != null) NIPTextBox.IsEnabled = (bool) !NoNIPCheckBox.IsChecked;
            NoNIPCheckBox.IsEnabled = true;
            if (NoREGONCheckBox.IsChecked != null) REGONTextBox.IsEnabled = (bool) !NoREGONCheckBox.IsChecked;
            NoREGONCheckBox.IsEnabled = true;
            if (NoBankAccountCheckBox.IsChecked != null)
                BankAccountNumberTextBox.IsEnabled = (bool) !NoBankAccountCheckBox.IsChecked;
            NoBankAccountCheckBox.IsEnabled = true;
            CommentsTextBox.IsEnabled = true;
        }

        private void NoNIPCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            EnableControls(sender, NIPTextBox);
        }

        private void NoREGONCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            EnableControls(sender, REGONTextBox);
        }

        private void NoBankAccountCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            EnableControls(sender, BankAccountNumberTextBox);
        }

        private void EnableControls(object sender, MaskedTextBox textBox)
        {
            var isChecked = ((CheckBox)sender).IsChecked;
            if (isChecked != null)
                textBox.IsEnabled = (bool)!isChecked;
        }
    }
}
