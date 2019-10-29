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

        public Invoice GetInvoice(int id)
        {
            using var ctx = new ApplicationContext();

            return ctx.Invoices
                .Include(x => x.PaymentType)
                .Include(x => x.InvoiceItems)
                .Include(x => x.Contractor)
                .Include(x => x.Seller)
                .FirstOrDefault(x => x.Id == id);
        }

        public Invoice GetLastInvoice()
        {
            using var ctx = new ApplicationContext();

            return ctx.Invoices.OrderByDescending(p => p.Id).FirstOrDefault();
        }

        public void DeleteInvoice(Invoice invoice)
        {
            using var ctx = new ApplicationContext();

            ctx.Invoices.Remove(invoice);
            ctx.SaveChanges();
        }
    }
}
