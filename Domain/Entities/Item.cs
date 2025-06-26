using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Item
    {
        public int Id { get; set; } 

        public int ProductId { get; set; }

        public Product Product { get; set; } = null!;

        public string ItemName { get; set; } = null!;

        public int Quantity { get; set; }
    }
}
