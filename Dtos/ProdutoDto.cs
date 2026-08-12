using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cardapio_digital.Dtos
{
    public class ProductDto
    {
        public required string Name { get; set; }

        public decimal Price { get; set; }

        public int CategoryId { get; set; }
    }
}
