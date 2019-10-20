using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EngineeringThesis.Core.Models;

namespace EngineeringThesis.Core.Services
{
    public class InvoiceItemService
    {
        public InvoiceItem GetItem(int id)
        {
            using var ctx = new ApplicationContext();

            return ctx.InvoiceItems.FirstOrDefault(x => x.Id == id);
        }
    }
}
