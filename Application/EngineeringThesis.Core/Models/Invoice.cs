using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EngineeringThesis.Core.Models.Enums;

namespace EngineeringThesis.Core.Models
{
    public class Invoice
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string InvoiceNumber { get; set; }
        public DateTime InvoiceDate { get; set; }
        public int ContractorId { get; set; }
        public int SellerId { get; set; }
        public int PaymentTypeId { get; set; } //ENUM
        public DateTime PaymentDeadline { get; set; }
        public string Comments { get; set; }

        [ForeignKey("ContractorId")]
        public Customer Contractor { get; set; }
        [ForeignKey("SellerId")]
        public Customer Seller { get; set; }
        [ForeignKey("PaymentTypeId")]
        public PaymentTypes PaymentType { get; set; }
        public ICollection<InvoiceItem> InvoiceItems { get; set; }

    }
}
