using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace cardapio_digital.Entities
{
    public class EscolaDto
    {
        
        public required string Nome { get; set; }

        public required string Endereco { get; set; } 

        public required string Telefone { get; set; } 

        public required string Turno { get; set; } 
    }
}