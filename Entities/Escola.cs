using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cardapio_digital.Enums;
namespace cardapio_digital.Entities
{
    public class Escola
    {
        public int Id { get; set; }

        public required string Nome { get; set; } 

        public required string Endereco { get; set; }

        public required string Telefone { get; set; }

        public required List<Turno> Turnos { get; set; } = new();
        
        public int UsuarioId { get; set; }

        public Usuario Usuario { get; set; }=null!;

        public ICollection<Filho> Filhos { get; set; } = new List<Filho>();

    }
}