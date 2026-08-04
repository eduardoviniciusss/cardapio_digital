using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cardapio_digital.Dtos
{
    public class CadastroFilhoDto
    {
        public required string Nome { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Telefone { get; set; } = string.Empty;
        public int EscolaId { get; set; }

    }
}