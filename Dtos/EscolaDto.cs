using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using cardapio_digital.Enums;

namespace cardapio_digital.Dtos
{
    public class EscolaDto
    {
        public required string Nome { get; set; }

        public required string Endereco { get; set; } 

        public required string Telefone { get; set; } 

        public required List<Turno> Turnos { get; set; } = new();

        public int UsuarioId { get; set; }
    
    }
}