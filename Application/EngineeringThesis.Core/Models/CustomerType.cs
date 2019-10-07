using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EngineeringThesis.Core.Models
{
    public class CustomerType
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
