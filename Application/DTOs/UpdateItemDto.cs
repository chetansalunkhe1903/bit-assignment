using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class UpdateItemDto
    {
        public string ItemName { get; set; } = null!;
        public int Quantity { get; set; }
    }
}
