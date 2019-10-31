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
        private readonly ApplicationContext _ctx;

        public InvoiceService(ApplicationContext context)
        {
            _ctx = context;
        }
        public List<Invoice> GetInvoices()
        {
            return _ctx.Invoices
                .Include(p => p.InvoiceItems)
                .Include(seller => seller.Seller)
                .Include(contractor => contractor.Contractor)
                .Include(paymentType => paymentType.PaymentType)
                .ToList();
        }

        public Invoice GetInvoice(int id)
        {
            return _ctx.Invoices
                .Include(x => x.PaymentType)
                .Include(x => x.InvoiceItems)
                .Include(x => x.Contractor)
                .Include(x => x.Seller)
                .FirstOrDefault(x => x.Id == id);
        }

        public Invoice GetLastInvoice()
        {
            return _ctx.Invoices.OrderByDescending(p => p.Id).FirstOrDefault();
        }

        public void DeleteInvoice(Invoice invoice)
        {
            _ctx.Invoices.Remove(invoice);
            _ctx.SaveChanges();
        }
    }
}
