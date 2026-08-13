using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cardapio_digital.Entities;

namespace cardapio_digital.Dtos
{
    public class ChildRegistrationResponseDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public required string Phone { get; set; }
        public int ParentId { get; set; }
        public ParentRegistrationResponseDto Parent { get; set; } = null!;
        public int SchoolId { get; set; }
        public SchoolResponseDto School { get; set; } = null!;
    }
}
