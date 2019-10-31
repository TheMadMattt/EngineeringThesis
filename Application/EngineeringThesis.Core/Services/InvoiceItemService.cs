using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EngineeringThesis.Core.Models;

namespace EngineeringThesis.Core.Services
{
    public class InvoiceItemService
    {
        private readonly ApplicationContext _ctx;
        public InvoiceItemService(ApplicationContext context)
        {
            _ctx = context;
        }
        public InvoiceItem GetInvoiceItem(int id)
        {
            return _ctx.InvoiceItems.FirstOrDefault(x => x.Id == id);
        }
    }
}
