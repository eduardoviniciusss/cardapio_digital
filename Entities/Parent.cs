using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cardapio_digital.Enums;

namespace cardapio_digital.Entities
{
    public class Parent
    {
        public int Id { get; set; }
        public string Name { get; set; }= string.Empty;
        public string Cpf { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public int UserId{ get; set; }
        public User User { get; set; } = null!; 
        public ICollection<Child> Children { get; set;} = new List<Child>();
    }
}
