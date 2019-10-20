using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using EngineeringThesis.Core.Models;
using EngineeringThesis.Core.Services;
using EngineeringThesis.Core.Utility;
using EngineeringThesis.Core.Utility.ShowDialogs;
using EngineeringThesis.UI.Navigation;
using EngineeringThesis.UI.ViewModel;
using MaterialDesignThemes.Wpf;

namespace EngineeringThesis.UI.View
{
    /// <summary>
    /// Interaction logic for InvoiceItemWindow.xaml
    /// </summary>
    public partial class InvoiceItemWindow : Window, IActivable
    {
        private readonly InvoiceItemService _invoiceItemService;
        public InvoiceItemViewModel InvoiceItemViewModel;
        public InvoiceItemWindow(InvoiceItemService invoiceItemService, InvoiceItemViewModel invoiceItemViewModel)
        {
            InitializeComponent();
            _invoiceItemService = invoiceItemService;
            InvoiceItemViewModel = invoiceItemViewModel;

            DataContext = InvoiceItemViewModel;
        }

        public Task ActivateAsync(object parameter)
        {
            if (parameter is InvoiceItem invoiceItem)
            {
                InvoiceItemViewModel.InvoiceItem = invoiceItem;
            }
            Title = !string.IsNullOrEmpty(InvoiceItemViewModel.InvoiceItem.Name) ? "Edycja produktu" : "Dodawanie produktu";

            return Task.CompletedTask;
        }

        private void AmountTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = Utility.IsTextNumeric(e.Text);
        }

        private void VATTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = Utility.IsTextNumeric(e.Text);
        }

        private void NetPriceTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(AmountTextBox.Text) && !string.IsNullOrEmpty(NetPriceTextBox.Text))
            {
                var sum = Convert.ToDecimal(AmountTextBox.Text) * Convert.ToDecimal(NetPriceTextBox.Value);
                NetSumTextBlock.Text = sum.ToString("C2", new CultureInfo("pl-PL"));
            }
            if (!string.IsNullOrEmpty(AmountTextBox.Text) && !string.IsNullOrEmpty(NetPriceTextBox.Text) && !string.IsNullOrEmpty(VATTextBox.Text))
            {
                var VAT = (Convert.ToDecimal(VATTextBox.Text) / 100) + 1;
                var sum = Convert.ToDecimal(AmountTextBox.Text) * Convert.ToDecimal(NetPriceTextBox.Value) * VAT;
                GrossSumTextBlock.Text = sum.ToString("C2", new CultureInfo("pl-PL"));
            }
        }

        private void AmountTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(AmountTextBox.Text) && !string.IsNullOrEmpty(NetPriceTextBox.Text))
            {
                var sum = Convert.ToDecimal(AmountTextBox.Text) * Convert.ToDecimal(NetPriceTextBox.Value);
                NetSumTextBlock.Text = sum.ToString("C2", new CultureInfo("pl-PL"));
            }
            if (!string.IsNullOrEmpty(AmountTextBox.Text) && !string.IsNullOrEmpty(NetPriceTextBox.Text) && !string.IsNullOrEmpty(VATTextBox.Text))
            {
                var VAT = (Convert.ToDecimal(VATTextBox.Text) / 100) + 1;
                var sum = Convert.ToDecimal(AmountTextBox.Text) * Convert.ToDecimal(NetPriceTextBox.Value) * VAT;
                GrossSumTextBlock.Text = sum.ToString("C2", new CultureInfo("pl-PL"));
            }
        }

        private void VATTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(AmountTextBox.Text) && !string.IsNullOrEmpty(NetPriceTextBox.Text) && !string.IsNullOrEmpty(VATTextBox.Text))
            {
                var VAT = (Convert.ToDecimal(VATTextBox.Text) / 100) + 1;
                var sum = Convert.ToDecimal(AmountTextBox.Text) * Convert.ToDecimal(NetPriceTextBox.Value) * VAT;
                GrossSumTextBlock.Text = sum.ToString("C2", new CultureInfo("pl-PL"));
            }
        }

        //VALIDATOR
//        private void Button_Click(object sender, RoutedEventArgs e)
//        {
//            ForceValidation();
//            if (!Validation.GetHasError(InvoiceItemNameTextBox))
//            {
//                MaterialMessageBox.Show("Żaden produkt nie został wybrany", "Zaznacz produkt");
//            }
//        }
        private void ForceValidation()
        {
            InvoiceItemNameTextBox.GetBindingExpression(TextBox.TextProperty)?.UpdateSource();
            NetPriceTextBox.GetBindingExpression(TextBox.TextProperty)?.UpdateSource();
            AmountTextBox.GetBindingExpression(TextBox.TextProperty)?.UpdateSource();
            VATTextBox.GetBindingExpression(TextBox.TextProperty)?.UpdateSource();
            UnitTextBox.GetBindingExpression(TextBox.TextProperty)?.UpdateSource();
        }

        private async void AddItemButton_Click(object sender, RoutedEventArgs e)
        {
            ForceValidation();
            if (ControlsHasError())
            {

            }
            else
            {
                await Forge.Forms.Show.Dialog("InvoiceItemDialogHost").For(new Information("Nie wszystkie pola zostały uzupełnione", "Uzupełnij pozostałe pola", "OK"));
            }
        }

        private bool ControlsHasError()
        {
            return !Validation.GetHasError(InvoiceItemNameTextBox) &&
                   !Validation.GetHasError(NetPriceTextBox) &&
                   !Validation.GetHasError(AmountTextBox) &&
                   !Validation.GetHasError(VATTextBox) &&
                   !Validation.GetHasError(UnitTextBox);
        }
    }
}
