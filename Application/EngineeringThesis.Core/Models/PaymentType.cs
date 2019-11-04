using System.ComponentModel.DataAnnotations;

namespace EngineeringThesis.Core.Models
{
    public class PaymentType
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
