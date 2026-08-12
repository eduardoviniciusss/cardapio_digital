using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cardapio_digital.Entities
{
    public class MenuProduct
    {
        public int MenuId { get; set; }
        public int ProductId { get; set; }
        public Menu Menu { get; set; }=null!;
        public Product Product { get; set; }=null!;
    }
}
