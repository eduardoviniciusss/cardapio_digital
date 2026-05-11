using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cardapio_digital.Entities
{
    public class Escola
    {
        public int Id { get; set; }

        public string? Nome { get; set; }

        public string? Endereco { get; set; }

        public string? Telefone { get; set; }

        public string? Turno { get; set; }
    }
}