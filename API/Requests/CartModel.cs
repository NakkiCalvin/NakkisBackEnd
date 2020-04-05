using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Requests
{
    public class CartModel
    {
        public int Id { get; set; }
        public int TotalQty { get; set; }
        public double TotalPrice { get; set; }
        public Guid UserId { get; set; }
        public int ProductId { get; set; }
        public bool increase { get; set; }
        public bool decrease { get; set; }

    }
}
