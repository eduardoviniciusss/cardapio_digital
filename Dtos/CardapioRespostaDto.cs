using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cardapio_digital.Entities;

namespace cardapio_digital.Dtos
{
    public class MenuResponseDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public School School { get; set; }= null!;

    }
}
