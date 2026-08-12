using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cardapio_digital.Enums;
namespace cardapio_digital.Entities
{
    public class School
    {
        public int Id { get; set; }

        public required string Name { get; set; } 

        public required string Address { get; set; }

        public required string Phone { get; set; }

        public required List<Shift> Shifts { get; set; } = new();
        
        public int UserId { get; set; }

        public User User { get; set; }=null!;

        public ICollection<Child> Children { get; set; } = new List<Child>();

        public ICollection<Product> Products { get; set; } = new List<Product>();

    }
}
