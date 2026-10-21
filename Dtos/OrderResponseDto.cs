using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cardapio_digital.Entities;


namespace cardapio_digital.Dtos
{
    public class OrderItemResponseDto
    {
        public int ProductId { get; set; }
        public required string Name { get; set; }
        public required string Category { get; set; }
        public decimal Price { get; set; }
    }

    public class OrderResponseDto
    {
        public int Id { get; set; }
        public int ChildId { get; set; }
        public required string ChildName { get; set; }
        public int MenuId { get; set; }
        public required string MenuName { get; set; }
        public DateTime Date { get; set; }
        public decimal Total { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<OrderItemResponseDto> Items { get; set; } = new();
    }
}
