using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cardapio_digital.Entities
{
    public class Child
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public string Phone { get; set; } = string.Empty;
        public int ParentId { get; set; }
        public Parent Parent { get; set; } = null!;
        public int SchoolId { get; set; }
        public School School { get; set; } = null!;

    }
}
