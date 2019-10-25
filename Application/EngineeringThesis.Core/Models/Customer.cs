using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Forge.Forms.Annotations;

namespace EngineeringThesis.Core.Models
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Value(Must.NotBeEmpty)]
        public string Name { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string StreetNumber { get; set; }
        public string? PhoneNumber { get; set; }
        public string NIP { get; set; }
        public string REGON { get; set; }
        public string BankAccountNumber { get; set; }
        public string? Comments { get; set; }
        public int CustomerTypeId { get; set; }

        [ForeignKey("CustomerTypeId")]
        public CustomerType CustomerType { get; set; }
    }
}
