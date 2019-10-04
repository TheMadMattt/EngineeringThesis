using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringThesis.Models
{
    class Customer
    {
        public int Id { get; set; }
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
        public int CustomerType { get; set; }
    }
}
