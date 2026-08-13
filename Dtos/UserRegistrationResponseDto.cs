using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cardapio_digital.Enums;

namespace cardapio_digital.Dtos
{
    public class UserRegistrationResponseDto
    {
      public int Id { get; set; }
      public required string Name { get; set; }
      public required string Email { get; set; }
      public UserRole Role { get; set; }

    }
}
