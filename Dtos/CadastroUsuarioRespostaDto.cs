using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cardapio_digital.Enums;

namespace cardapio_digital.Dtos
{
    public class CadastroUsuarioRespostaDto
    {
      public int Id { get; set; }
      public required string Nome { get; set; }
      public required string Email { get; set; }
      public PerfilUsuario Perfil { get; set; }

    }
}