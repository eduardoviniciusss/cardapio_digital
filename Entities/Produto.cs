using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cardapio_digital.Entities
{
    public class Produto
    {
        public int Id { get; set; }
        public string?Nome { get; set; }
        public string?Tipo { get; set; }
        public decimal?Preco { get; set; }
    }
}