using System;
using System.Collections.Generic;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using EngineeringThesis.Core.Models;
using EngineeringThesis.Core.Utility;
using EngineeringThesis.UI.Navigation;
using EngineeringThesis.UI.ViewModel;

namespace EngineeringThesis.UI.View
{
    /// <summary>
    /// Interaction logic for AddCustomerWindow.xaml
    /// </summary>
    public partial class AddCustomerWindow : Window, IActivable
    {
        public AddCustomerViewModel CustomerViewModel;
        public AddCustomerWindow(AddCustomerViewModel customerViewModel)
        {
            InitializeComponent();
            CustomerViewModel = customerViewModel;

            CustomerTypeComboBox.ItemsSource = CustomerViewModel.GetCustomerTypes();
            DataContext = CustomerViewModel;
        }

        public Task ActivateAsync(object parameter)
        {
            if (parameter is Customer customer)
            {
                CustomerViewModel.CustomerWithRef = customer;
                CustomerViewModel.Customer = new Customer
                {
                    Name = customer.Name,
                    ZipCode = customer.ZipCode,
                    City = customer.City,
                    Street = customer.Street,
                    StreetNumber = customer.StreetNumber,
                    PhoneNumber = customer.PhoneNumber,
                    NIP = customer.NIP,
                    REGON = customer.REGON,
                    BankAccountNumber = customer.BankAccountNumber,
                    Comments = customer.Comments,
                    CustomerTypeId = customer.CustomerTypeId
                };
                CustomerViewModel.SplitAddress(customer.StreetNumber);
                CustomerViewModel.IsUpdate = true;

                PrepareControls();
            }
            else
            {
                CustomerViewModel.CustomerWithRef = new Customer();
                CustomerViewModel.Customer = new Customer();
            }

            return Task.CompletedTask;
        }

        private void PrepareControls()
        {
            HasNIPCheckBox.IsChecked = CustomerViewModel.Customer.NIP == null;
            HasREGONCheckBox.IsChecked = CustomerViewModel.Customer.REGON == null;
            HasBankAccountCheckBox.IsChecked = CustomerViewModel.Customer.BankAccountNumber == null;
        }

        private void PhoneNumberTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = Utility.IsTextNumeric(e.Text);
        }

        private void AddCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            if (HasNIPCheckBox.IsChecked == true)
            {
                CustomerViewModel.Customer.NIP = null;
            }

            if (HasREGONCheckBox.IsChecked == true)
            {
                CustomerViewModel.Customer.REGON = null;
            }

            if (HasBankAccountCheckBox.IsChecked == true)
            {
                CustomerViewModel.Customer.BankAccountNumber = null;
            }

            CustomerViewModel.BindToRefObject();

            if (CustomerViewModel.IsUpdate)
            {
                CustomerViewModel.UpdateCustomer();
            }
            else
            {
                CustomerViewModel.SaveCustomer();
            }
            
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
