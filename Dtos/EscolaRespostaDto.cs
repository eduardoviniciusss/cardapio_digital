using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cardapio_digital.Enums;

namespace cardapio_digital.Dtos
{
    public class EscolaRespostaDto
    {
        public int Id { get; set; }
        public required string Nome { get; set; }
        public required string Telefone { get; set; }
        public required string Endereco { get; set; }
        public List<Turno> Turnos { get; set; } = new();



    }
}