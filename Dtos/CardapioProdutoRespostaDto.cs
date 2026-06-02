using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cardapio_digital.Dtos
{
    public class CardapioProdutoRespostaDto
    {
       public int CardapioId { get; set; }
        public required string Cardapio { get; set; }

        public int ProdutoId { get; set; } 
        public required string Produto { get; set; }

        public required string Categoria { get; set; }
    }
}