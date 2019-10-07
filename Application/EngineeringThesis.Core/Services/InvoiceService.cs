﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using EngineeringThesis.Core.Models;

namespace EngineeringThesis.Core.Services
{
    public class InvoiceService
    {
        private readonly EngineeringThesisContext _ctx;
        public InvoiceService()
        {
        }

        public void testAdd()
        {
            using (var ctx = new EngineeringThesisContext())
            {
                var customer = ctx.Customers.FirstOrDefault(x => x.Id == 1);
                var paymentType = ctx.PaymentTypes.FirstOrDefault(x => x.Id == 1);

                Invoice invoice = new Invoice
                {
                    InvoiceNumber = "01/2019", 
                    InvoiceDate = DateTime.Now,
                    Contractor = customer,
                    Seller = customer,
                    PaymentType = paymentType,
                    PaymentDeadline = new DateTime(2019,11,10)
                };

                ctx.Invoices.Add(invoice);
                ctx.SaveChanges();
            }
        }

    }
}
