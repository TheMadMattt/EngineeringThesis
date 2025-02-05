﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EngineeringThesis.Core.Models
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string StreetNumber { get; set; }
#nullable enable
        public string? PhoneNumber { get; set; }
#nullable disable
        public string NIP { get; set; }
        public string REGON { get; set; }
        public string BankAccountNumber { get; set; }
#nullable enable
        public string? Comments { get; set; }
#nullable disable
        public int CustomerTypeId { get; set; }

        [ForeignKey("CustomerTypeId")]
        public CustomerType CustomerType { get; set; }
    }
}
