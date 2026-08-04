using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace cardapio_digital.Dtos
{
    public class CadastroPaisDto
    {
        public required string Nome { get; set; }= string.Empty;
        public string Cpf { get; set; } = string.Empty;

        public string Telefone { get; set; } = string.Empty;

    }
}