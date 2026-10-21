using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cardapio_digital.Dtos
{
    public class ChildMenuDto
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public List<ChildMenuProductDto> Products { get; set; } = [];
    }
}