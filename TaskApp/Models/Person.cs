using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TaskApp.Models
{
    public class Person
    {
        [Key]
        public long Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public long? AddressId { get; set; }
        public Address Address { get; set; }

    }
}
