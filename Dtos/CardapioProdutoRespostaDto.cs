using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cardapio_digital.Dtos
{
    public class MenuProductResponseDto
    {
       public int MenuId { get; set; }
        public required string Menu { get; set; }

        public int ProductId { get; set; } 
        public required string Product { get; set; }

        public required string Category { get; set; }
    }
}
