using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cardapio_digital.Entities
{
    public class Produto
    {
        public int Id { get; set; }

        public required string Nome { get; set; }

        public decimal Preco { get; set; }

        public int CategoriaId { get; set; }

        public Categoria Categoria { get; set; }= null!;

        public int EscolaId { get; set; }
        public Escola Escola { get; set; }= null!;
    }
}