using System.Collections.Generic;
using System.Linq;
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

        public void SaveInvoice(Invoice invoice)
        {
            _ctx.Invoices.Add(invoice);
            _ctx.SaveChanges();
        }

        public void UpdateInvoice(Invoice invoice)
        {
            var oldInvoice = _ctx.Customers.Find(invoice.Id);

            _ctx.Entry(oldInvoice).CurrentValues.SetValues(invoice);
            _ctx.SaveChanges();
        }

        public LastInvoiceNumber FindLastInvoiceNumber(string lastInvoiceNumber)
        {
            return _ctx.LastInvoiceNumbers.FirstOrDefault(x => x.InvoiceNumber.Contains(lastInvoiceNumber));
        }

        public void UpdateLastInvoiceNumber(LastInvoiceNumber lastNumber)
        {
            var oldLastNumber = _ctx.LastInvoiceNumbers.Find(lastNumber.Id);

            _ctx.Entry(oldLastNumber).CurrentValues.SetValues(lastNumber);

            _ctx.SaveChanges();
        }

        public void SaveLastInvoiceNumber(LastInvoiceNumber newLastNumber)
        {
            _ctx.LastInvoiceNumbers.Add(newLastNumber);

            _ctx.SaveChanges();
        }

        public void DeleteLastInvoiceNumber(LastInvoiceNumber lastInvoiceNumber)
        {
            _ctx.LastInvoiceNumbers.Remove(lastInvoiceNumber);
            _ctx.SaveChanges();
        }
    }
}
