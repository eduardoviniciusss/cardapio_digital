using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cardapio_digital.Dtos
{
    public class ChildRegistrationDto
    {
        public required string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public string Phone { get; set; } = string.Empty;
        public int SchoolId { get; set; }
        public int ParentId { get; set; }
    }
 
}
