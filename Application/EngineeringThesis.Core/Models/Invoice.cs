using System;
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
        [ForeignKey(nameof(Customer))]
        public int ContractorId { get; set; }
        [ForeignKey(nameof(Customer))]
        public int SellerId { get; set; }
        [ForeignKey(nameof(PaymentTypes))]
        public int PaymentId { get; set; } //ENUM
        public DateTime PaymentDeadline { get; set; }
        public string Comments { get; set; }

    }
}
