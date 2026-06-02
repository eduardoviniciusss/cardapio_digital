using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cardapio_digital.Entities;

namespace cardapio_digital.Dtos
{
    public class CardapioRespostaDto
    {
        public int Id { get; set; }
        public required string Nome { get; set; }
        public Escola Escola { get; set; }= null!;

    }
}