using System;
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
                var customer = ctx.Customers;

                customer.ToString();

            }
        }

    }
}
