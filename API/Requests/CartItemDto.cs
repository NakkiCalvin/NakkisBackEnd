using BLL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Requests
{
    public class CartItemDto
    {
        public Product Item { get; set; }
        public int Qty { get; set; }
        public double Price { get; set; }
    }
}
