using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cardapio_digital.Entities
{
    public class ProdutoDto
    {
        public required string Nome { get; set; }

        public decimal Preco { get; set; }

        public int CategoriaId { get; set; }
    }
}