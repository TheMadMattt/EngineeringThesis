using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EngineeringThesis.Core.Models
{
    public class InvoiceItem
    {
        [Key]
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        [Required]
        public string Name { get; set; }
        public string PKWiU { get; set; }
        public string Unit { get; set; }
        public int NetPrice { get; set; }
        public int Amount { get; set; }
        public int VATSum { get; set; } // ????
        public int NetSum { get; set; }
        public int GrossSum { get; set; }
        public string Comments { get; set; }

        [ForeignKey("InvoiceId")]
        public Invoice Invoice { get; set; }

    }
}
