﻿using System;
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

namespace EngineeringThesis.UI.View
{
    /// <summary>
    /// Interaction logic for InvoiceItemWindow.xaml
    /// </summary>
    public partial class InvoiceItemWindow : IActivable
    {
        public InvoiceItemViewModel ViewModel;
        public InvoiceItemWindow(InvoiceItemViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;

            DataContext = ViewModel;
        }

        public Task ActivateAsync(object parameter)
        {
            if (parameter is InvoiceItem invoiceItem)
            {
                if (!string.IsNullOrEmpty(invoiceItem.Name))
                {
                    ViewModel.InvoiceItemWithRef = invoiceItem;
                    ViewModel.InvoiceItem = new InvoiceItemDisplayModel
                    {
                        Name = invoiceItem.Name,
                        Amount = invoiceItem.Amount,
                        NetPrice = invoiceItem.NetPrice,
                        VAT = invoiceItem.VAT,
                        Unit = invoiceItem.Unit,
                        PKWiU = invoiceItem.PKWiU,
                        Comments = invoiceItem.Comments,
                        NetSum = invoiceItem.NetSum,
                        GrossSum = invoiceItem.GrossSum
                    };
                }
                else
                {
                    ViewModel.InvoiceItem = new InvoiceItemDisplayModel();
                    ViewModel.InvoiceItemWithRef = invoiceItem;
                }
                

            }
            Title = !string.IsNullOrEmpty(ViewModel.InvoiceItem.Name) ? "Edycja produktu" : "Dodawanie produktu";

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
                ViewModel.InvoiceItem.NetSum = sum.ToString("#.00");
                NetSumTextBlock.Text = sum.ToString("C2");
            }
            if (!string.IsNullOrEmpty(AmountTextBox.Value.ToString()) && !string.IsNullOrEmpty(NetPriceTextBox.Value.ToString()) && !string.IsNullOrEmpty(VATTextBox.Text))
            {
                var vat = (Convert.ToDecimal(VATTextBox.Text) / 100) + 1;
                var sum = Convert.ToDecimal(AmountTextBox.Value) * Convert.ToDecimal(NetPriceTextBox.Value) * vat;
                ViewModel.InvoiceItem.GrossSum = sum.ToString("#.00");
                GrossSumTextBlock.Text = sum.ToString("C2");
            }
        }

        private void AmountTextBox_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double?> e)
        {
            if (!string.IsNullOrEmpty(AmountTextBox.Value.ToString()) && !string.IsNullOrEmpty(NetPriceTextBox.Value.ToString()))
            {
                var sum = Convert.ToDecimal(AmountTextBox.Value) * Convert.ToDecimal(NetPriceTextBox.Value); 
                ViewModel.InvoiceItem.NetSum = sum.ToString("#.00");
                NetSumTextBlock.Text = sum.ToString("C2");
            }
            if (!string.IsNullOrEmpty(AmountTextBox.Value.ToString()) && !string.IsNullOrEmpty(NetPriceTextBox.Value.ToString()) && !string.IsNullOrEmpty(VATTextBox.Text))
            {
                var vat = (Convert.ToDecimal(VATTextBox.Text) / 100) + 1;
                var sum = Convert.ToDecimal(AmountTextBox.Value) * Convert.ToDecimal(NetPriceTextBox.Value) * vat;
                ViewModel.InvoiceItem.GrossSum = sum.ToString("#.00");
                GrossSumTextBlock.Text = sum.ToString("C2");
            }
        }

        private void VATTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(AmountTextBox.Value.ToString()) && !string.IsNullOrEmpty(NetPriceTextBox.Value.ToString()) && !string.IsNullOrEmpty(VATTextBox.Text))
            {
                var vat = (Convert.ToDecimal(VATTextBox.Text) / 100) + 1;
                var sum = Convert.ToDecimal(AmountTextBox.Value) * Convert.ToDecimal(NetPriceTextBox.Value) * vat;
                ViewModel.InvoiceItem.GrossSum = sum.ToString("#.00");
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
                Close();
            }
            else
            {
                await Forge.Forms.Show.Dialog("InvoiceItemDialogHost").For(new Information("Nie wszystkie pola zostały uzupełnione", "Uzupełnij pozostałe pola", "OK"));
            }
        }

        private void UpdateInvoiceItem()
        {
            ViewModel.BindToRefObject();

            if (!string.IsNullOrEmpty(GrossSumTextBlock.Text) && !string.IsNullOrEmpty(NetSumTextBlock.Text))
            {
                var vatSum = Convert.ToDecimal(ViewModel.InvoiceItem.GrossSum) - Convert.ToDecimal(ViewModel.InvoiceItem.NetSum);
                ViewModel.InvoiceItemWithRef.VATSum = vatSum.ToString("#.00");
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
            Close();
        }
    }
}
