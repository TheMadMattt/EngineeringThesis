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
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using EngineeringThesis.Core.Models;
using EngineeringThesis.Core.Services;
using EngineeringThesis.Core.Utility;
using EngineeringThesis.Core.Utility.ShowDialogs;
using EngineeringThesis.Core.ViewModel;
using EngineeringThesis.UI.Navigation;
using MaterialDesignThemes.Wpf;

namespace EngineeringThesis.UI.View
{
    /// <summary>
    /// Interaction logic for InvoiceItemWindow.xaml
    /// </summary>
    public partial class InvoiceItemWindow : Window, IActivable
    {
        public InvoiceItemViewModel InvoiceItemViewModel;
        public InvoiceItemWindow(InvoiceItemViewModel invoiceItemViewModel)
        {
            InitializeComponent();
            InvoiceItemViewModel = invoiceItemViewModel;

            DataContext = InvoiceItemViewModel;
        }

        public Task ActivateAsync(object parameter)
        {
            if (parameter is InvoiceItem invoiceItem)
            {
                InvoiceItemViewModel.InvoiceItemWithRef = invoiceItem;
                InvoiceItemViewModel.InvoiceItem = new InvoiceItem
                {
                    Name = invoiceItem.Name,
                    Amount = invoiceItem.Amount,
                    NetPrice = invoiceItem.NetPrice.Replace(".",","),
                    VAT = invoiceItem.VAT,
                    Unit = invoiceItem.Unit,
                    PKWiU = invoiceItem.PKWiU,
                    Comments = invoiceItem.Comments,
                    NetSum = invoiceItem.NetSum,
                    GrossSum = invoiceItem.GrossSum
                };

            }
            Title = !string.IsNullOrEmpty(InvoiceItemViewModel.InvoiceItem.Name) ? "Edycja produktu" : "Dodawanie produktu";

            return Task.CompletedTask;
        }

        private void VATTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = Utility.IsTextNumeric(e.Text);
        }

        private void NetPriceTextBox_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double?> e)
        {
            if (!string.IsNullOrEmpty(AmountTextBox.Value.ToString()) && !string.IsNullOrEmpty(NetPriceTextBox.Value.ToString()))
            {
                var sum = Convert.ToDecimal(AmountTextBox.Value) * Convert.ToDecimal(NetPriceTextBox.Value);
                InvoiceItemViewModel.InvoiceItem.NetSum = sum.ToString("#.00");
                NetSumTextBlock.Text = sum.ToString("C2");
            }
            if (!string.IsNullOrEmpty(AmountTextBox.Value.ToString()) && !string.IsNullOrEmpty(NetPriceTextBox.Value.ToString()) && !string.IsNullOrEmpty(VATTextBox.Text))
            {
                var VAT = (Convert.ToDecimal(VATTextBox.Text) / 100) + 1;
                var sum = Convert.ToDecimal(AmountTextBox.Value) * Convert.ToDecimal(NetPriceTextBox.Value) * VAT;
                InvoiceItemViewModel.InvoiceItem.GrossSum = sum.ToString("#.00");
                GrossSumTextBlock.Text = sum.ToString("C2");
            }
        }

        private void AmountTextBox_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double?> e)
        {
            if (!string.IsNullOrEmpty(AmountTextBox.Value.ToString()) && !string.IsNullOrEmpty(NetPriceTextBox.Value.ToString()))
            {
                var sum = Convert.ToDecimal(AmountTextBox.Value) * Convert.ToDecimal(NetPriceTextBox.Value); 
                InvoiceItemViewModel.InvoiceItem.NetSum = sum.ToString("#.00");
                NetSumTextBlock.Text = sum.ToString("C2");
            }
            if (!string.IsNullOrEmpty(AmountTextBox.Value.ToString()) && !string.IsNullOrEmpty(NetPriceTextBox.Value.ToString()) && !string.IsNullOrEmpty(VATTextBox.Text))
            {
                var VAT = (Convert.ToDecimal(VATTextBox.Text) / 100) + 1;
                var sum = Convert.ToDecimal(AmountTextBox.Value) * Convert.ToDecimal(NetPriceTextBox.Value) * VAT;
                InvoiceItemViewModel.InvoiceItem.GrossSum = sum.ToString("#.00");
                GrossSumTextBlock.Text = sum.ToString("C2");
            }
        }

        private void VATTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(AmountTextBox.Value.ToString()) && !string.IsNullOrEmpty(NetPriceTextBox.Value.ToString()) && !string.IsNullOrEmpty(VATTextBox.Text))
            {
                var VAT = (Convert.ToDecimal(VATTextBox.Text) / 100) + 1;
                var sum = Convert.ToDecimal(AmountTextBox.Value) * Convert.ToDecimal(NetPriceTextBox.Value) * VAT;
                InvoiceItemViewModel.InvoiceItem.GrossSum = sum.ToString("#.00");
                GrossSumTextBlock.Text = sum.ToString("C2");
            }
        }

        private void ForceValidation()
        {
            InvoiceItemNameTextBox.GetBindingExpression(TextBox.TextProperty)?.UpdateSource();
            VATTextBox.GetBindingExpression(TextBox.TextProperty)?.UpdateSource();
            UnitTextBox.GetBindingExpression(TextBox.TextProperty)?.UpdateSource();
        }

        private async void AddItemButton_Click(object sender, RoutedEventArgs e)
        {
            ForceValidation();
            if (ControlsHasError())
            {
                UpdateInvoiceItem();
                this.Close();
            }
            else
            {
                await Forge.Forms.Show.Dialog("InvoiceItemDialogHost").For(new Information("Nie wszystkie pola zostały uzupełnione", "Uzupełnij pozostałe pola", "OK"));
            }
        }

        private void UpdateInvoiceItem()
        {
            InvoiceItemViewModel.BindToRefObject();

            if (!string.IsNullOrEmpty(GrossSumTextBlock.Text) && !string.IsNullOrEmpty(NetSumTextBlock.Text))
            {
                var VATSum = Convert.ToDecimal(InvoiceItemViewModel.InvoiceItem.GrossSum) - Convert.ToDecimal(InvoiceItemViewModel.InvoiceItem.NetSum);
                InvoiceItemViewModel.InvoiceItemWithRef.VATSum = VATSum.ToString("#.00");
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

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
