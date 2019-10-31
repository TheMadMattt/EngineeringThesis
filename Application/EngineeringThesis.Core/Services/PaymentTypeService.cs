using System.Collections.Generic;
using System.Linq;
using EngineeringThesis.Core.Models;

namespace EngineeringThesis.Core.Services
{
    public class PaymentTypeService
    {
        private readonly ApplicationContext _ctx;

        public PaymentTypeService(ApplicationContext context)
        {
            _ctx = context;
        }
        public List<PaymentType> GetPaymentTypes()
        {
            return _ctx.PaymentTypes.ToList();
        }
    }
}
