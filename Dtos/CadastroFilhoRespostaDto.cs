using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cardapio_digital.Entities;

namespace cardapio_digital.Dtos
{
    public class CadastroFilhoRespostaDto
    {
        public int Id { get; set; }
        public required string Nome { get; set; }
        public DateTime DataNascimento { get; set; }
        public required string Telefone { get; set; }
        public int PaiId { get; set; }
        public CadastroPaisRespondeDto Pais { get; set; } = null!;
        public int EscolaId { get; set; }
        public EscolaRespostaDto Escola { get; set; } = null!;
    }
}