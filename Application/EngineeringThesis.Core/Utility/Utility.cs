﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using EngineeringThesis.Core.Models;

namespace EngineeringThesis.Core.Utility
{
    public class Utility
    {
        public static bool IsTextNumeric(string text)
        {
            Regex regex = new Regex("[^0-9]+");
            return regex.IsMatch(text);
        }

        public static bool IsNotCustomerNullOrEmpty(Customer obj)
        {
            return !string.IsNullOrEmpty(obj.Name) 
                   && !string.IsNullOrEmpty(obj.City) 
                   && !string.IsNullOrEmpty(obj.Street)
                   && !string.IsNullOrEmpty(obj.StreetNumber);
        }

        public static bool IsNotInvoiceItemNullOrEmpty(InvoiceItem obj)
        {
            return !string.IsNullOrEmpty(obj.Name)
                   && !string.IsNullOrEmpty(obj.Unit)
                   && !string.IsNullOrEmpty(obj.NetPrice)
                   && !string.IsNullOrEmpty(obj.VATSum)
                   && !string.IsNullOrEmpty(obj.NetSum) 
                   && !string.IsNullOrEmpty(obj.GrossSum)
                   && obj.Amount > 0
                   && obj.VAT > 0;
        }

        public static bool IsNotInvoiceNullOrEmpty(Invoice obj)
        {
            return !string.IsNullOrEmpty(obj.InvoiceNumber)
                   && !string.IsNullOrEmpty(obj.InvoiceDate.ToShortDateString())
                   && !string.IsNullOrEmpty(obj.PaymentDeadline.ToShortDateString())
                   && obj.InvoiceItems.Count > 0;
        }

        public struct CustomerStruct
        {
            public Customer Customer;
            public bool IsContractor;
        }

        public enum InvoiceTypeTemplateEnum
        {
            Original,
            Copy,
            Proforma,
            Correction
        }
    }
}

