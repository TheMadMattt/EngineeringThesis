﻿using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using EngineeringThesis.Core.Models;
using EngineeringThesis.Core.Models.DisplayModels;
using EngineeringThesis.Core.Utility;
using EngineeringThesis.Core.Utility.ShowDialogs;
using EngineeringThesis.Core.ViewModel;
using EngineeringThesis.UI.Navigation;

namespace EngineeringThesis.UI.View
{
    /// <summary>
    /// Interaction logic for AddCustomerWindow.xaml
    /// </summary>
    public partial class AddCustomerWindow : Window, IActivable
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
                ViewModel.Customer = new CustomerDisplayModel
                {
                    Name = customer.Name,
                    ZipCode = customer.ZipCode,
                    City = customer.City,
                    Street = customer.Street,
                    PhoneNumber = customer.PhoneNumber,
                    NIP = customer.NIP,
                    REGON = customer.REGON,
                    BankAccountNumber = customer.BankAccountNumber,
                    Comments = customer.Comments,
                    CustomerTypeId = customer.CustomerTypeId
                };
                ViewModel.SplitAddress(customer.StreetNumber);
                ViewModel.IsUpdate = true;

                PrepareControls();
            }
            else
            {
                ViewModel.CustomerWithRef = new Customer();
                ViewModel.Customer = new CustomerDisplayModel();
            }

            return Task.CompletedTask;
        }

        private void PrepareControls()
        {
            NoNIPCheckBox.IsChecked = ViewModel.Customer.NIP == null;
            NoREGONCheckBox.IsChecked = ViewModel.Customer.REGON == null;
            NoBankAccountCheckBox.IsChecked = ViewModel.Customer.BankAccountNumber == null;
            CustomerTypeComboBox.SelectedItem =
                ViewModel.CustomerTypes.Find(x => x.Id == ViewModel.Customer.CustomerTypeId);
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

                if (ViewModel.IsUpdate)
                {
                    ViewModel.UpdateCustomer();
                }
                else
                {
                    ViewModel.SaveCustomer();
                }

                this.Close();
            }
            else
            {
                await Forge.Forms.Show.Dialog("AddCustomerDialogHost").For(
                    new Information("Nie wszystkie pola zostały uzupełnione", "Uzupełnij pozostałe pola", "OK"));
            }

        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
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
        }

        private void NoREGONCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (NoREGONCheckBox.IsChecked == true)
            {
                var binding = REGONTextBox?.GetBindingExpression(TextBox.TextProperty);
                Validation.ClearInvalid(binding);
            }
        }

        private void NoBankAccountCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (NoBankAccountCheckBox.IsChecked == true)
            {
                var binding = BankAccountNumberTextBox?.GetBindingExpression(TextBox.TextProperty);
                Validation.ClearInvalid(binding);
            }
        }
    }
}
