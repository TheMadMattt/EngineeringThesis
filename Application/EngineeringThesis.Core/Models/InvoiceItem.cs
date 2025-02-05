﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EngineeringThesis.Core.Models
{
    public class InvoiceItem
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int InvoiceId { get; set; }
        [Required]
        public string Name { get; set; }
#nullable enable
        public string? PKWiU { get; set; }
#nullable disable
        public string Unit { get; set; }
        public string NetPrice { get; set; }
        public int Amount { get; set; }
        public int VAT { get; set; }
        public string VATSum { get; set; }
        public string NetSum { get; set; }
        public string GrossSum { get; set; }
#nullable enable
        public string? Comments { get; set; }
#nullable disable

        [ForeignKey("InvoiceId")]
        public Invoice Invoice { get; set; }

    }
}
