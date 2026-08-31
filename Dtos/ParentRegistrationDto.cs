using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace cardapio_digital.Dtos
{
    public class ParentRegistrationDto
    {
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Cpf { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
    }

}
