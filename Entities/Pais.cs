using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cardapio_digital.Enums;

namespace cardapio_digital.Entities
{
    public class Pais
    {
        public int Id { get; set; }
        public string Nome { get; set; }= string.Empty;
        public string Cpf { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public int UsuarioId{ get; set; }
        public Usuario Usuario { get; set; } = null!; 
        public ICollection<Filho> Filhos { get; set;} = new List<Filho>();
    }
}