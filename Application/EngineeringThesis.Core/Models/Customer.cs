using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EngineeringThesis.Core.Models.Enums;

namespace EngineeringThesis.Core.Models
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int Name { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public int StreetNumber { get; set; }
        public int SuiteNumber { get; set; }
        public string PhoneNumber { get; set; }
        public int NIP { get; set; }
        public int REGON { get; set; }
        public int BankAccountNumber { get; set; }
        [ForeignKey(nameof(CustomerTypes))]
        public int CustomerTypeId { get; set; }
    }
}
