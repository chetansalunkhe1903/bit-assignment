using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class CreateProductDto
    {
        public string ProductName { get; set; } = null!;
        public string CreatedBy { get; set; } = null!;
    }
}
