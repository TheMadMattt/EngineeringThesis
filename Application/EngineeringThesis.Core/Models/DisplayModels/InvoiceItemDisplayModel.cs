using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EngineeringThesis.Core.Models.DisplayModels
{
    public class InvoiceItemDisplayModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int InvoiceId { get; set; }
        [Required]
        public string Name { get; set; }
        public string? PKWiU { get; set; }
        public string Unit { get; set; }
        public string NetPrice { get; set; }
        public int Amount { get; set; }
        public int VAT { get; set; }
        public string VATSum { get; set; }
        public string NetSum { get; set; }
        public string GrossSum { get; set; }
        public string? Comments { get; set; }

        [ForeignKey("InvoiceId")]
        public Invoice Invoice { get; set; }

    }
}
