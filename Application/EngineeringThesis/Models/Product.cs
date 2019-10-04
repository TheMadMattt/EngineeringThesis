using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringThesis.Models
{
    class Product
    {
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public string Name { get; set; }
        public string PKWiU { get; set; }
        public string Unit { get; set; }
        public int NetPrice { get; set; }
        public int Amount { get; set; }
        public int VATSum { get; set; } // ????
        public int NetSum { get; set; }
        public int GrossSum { get; set; }
        public string Comments { get; set; }

    }
}
