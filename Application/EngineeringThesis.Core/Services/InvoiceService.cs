using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using EngineeringThesis.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace EngineeringThesis.Core.Services
{
    public class InvoiceService
    {
        public InvoiceService()
        {

        }

        public List<Invoice> TestAdd()
        {
            using var ctx = new EngineeringThesisContext();

            return ctx.Invoices
                .Include(p => p.InvoiceItems)
                .Include(seller => seller.Seller)
                .Include(contractor => contractor.Contractor)
                .Include(paymentType => paymentType.PaymentType)
                .ToList();
        }
    }
}
