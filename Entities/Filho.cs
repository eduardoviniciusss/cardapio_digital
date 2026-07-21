using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cardapio_digital.Entities
{
    public class Filho
    {
        public int Id { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Telefone { get; set; } = string.Empty;
        public int PaisId { get; set; }
        public Pais Pais { get; set; } = null!;
        public int EscolaId { get; set; }
        public Escola Escola { get; set; } = null!;




    }
}