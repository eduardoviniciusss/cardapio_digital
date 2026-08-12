using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cardapio_digital.Entities
{
    public class Menu
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public int SchoolId { get; set; }

        public School School { get; set; }= null!;
    }
}
