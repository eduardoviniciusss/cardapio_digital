using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cardapio_digital.Entities
{
    public class CardapioProduto
    {
        public int CardapioId { get; set; }
        public int ProdutoId { get; set; }
        public Cardapio Cardapio { get; set; }=null!;
        public Produto Produto { get; set; }=null!;
    }
}