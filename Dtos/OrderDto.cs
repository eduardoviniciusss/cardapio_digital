using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cardapio_digital.Entities;

namespace cardapio_digital.Dtos
{
    public class OrderDto
    {
        public int ChildId { get; set; }

        public int MenuId { get; set; }

        // Dia do lanche. Só a data importa; a hora é ignorada.
        public DateTime Date { get; set; }

        public List<int> ProductIds { get; set; } = new();
    }
}
