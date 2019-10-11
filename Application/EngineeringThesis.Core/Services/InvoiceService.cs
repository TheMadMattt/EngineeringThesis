using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EngineeringThesis.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace EngineeringThesis.Core.Services
{
    public class InvoiceService
    {
        public List<Invoice> GetInvoices()
        {
            using var ctx = new ApplicationContext();

            return ctx.Invoices
                .Include(p => p.InvoiceItems)
                .Include(seller => seller.Seller)
                .Include(contractor => contractor.Contractor)
                .Include(paymentType => paymentType.PaymentType)
                .ToList();
        }
    }
}
