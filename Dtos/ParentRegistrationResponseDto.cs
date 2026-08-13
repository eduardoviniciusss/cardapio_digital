using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cardapio_digital.Entities;

namespace cardapio_digital.Dtos
{
    public class ParentRegistrationResponseDto
    {
        public string Name { get; set; } = string.Empty;
        public int Id { get; set; }
        public string Cpf { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        
    }
}
