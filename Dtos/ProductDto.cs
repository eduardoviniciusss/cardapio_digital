using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cardapio_digital.Dtos
{
    public class ProductDto
    {
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public int SchoolId { get; set; }
    }

}
