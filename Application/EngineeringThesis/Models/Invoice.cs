using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringThesis.Models
{
    class Invoice
    {
        public int Id { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime InvoiceDate { get; set; }
        public int ContractorId { get; set; }
        public int SellerId { get; set; }
        public int PaymentId { get; set; } //ENUM
        public DateTime PaymentDeadline { get; set; }
        public string Comments { get; set; }

    }
}
