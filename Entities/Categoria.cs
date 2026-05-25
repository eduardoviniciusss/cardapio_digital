using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cardapio_digital.Entities
{
    public class Categoria
    {
        public int Id { get; set; }

        public required string Nome { get; set; }
    }
}