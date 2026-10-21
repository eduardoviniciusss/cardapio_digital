using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cardapio_digital.Entities
{
    
    public class Order
    {
        public int Id { get; set; }

        public int ChildId { get; set; }
        public Child Child { get; set; } = null!;

        public int MenuId { get; set; }
        public string MenuName { get; set; } = string.Empty;

        // Dia do lanche (só a data, sem hora)
        public DateTime Date { get; set; }

        public decimal Total { get; set; }

        public DateTime CreatedAt { get; set; }

        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
    }
}
