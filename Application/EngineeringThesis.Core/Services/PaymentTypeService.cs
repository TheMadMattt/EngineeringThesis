using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EngineeringThesis.Core.Models;

namespace EngineeringThesis.Core.Services
{
    public class PaymentTypeService
    {
        public List<PaymentType> GetPaymentTypes()
        {
            using var ctx = new ApplicationContext();

            return ctx.PaymentTypes.ToList();
        }
    }
}
