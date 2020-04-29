using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Entities
{
    public class CartItem
    {
        public int CartId { get; set; }
        public Cart Cart { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        //public int? VariantId { get; set; }
        //public Variant Variant { get; set; }
        public int Qty { get; set; }
        public double Price { get; set; }
        public DateTimeOffset FirstSeenDate { get; set; }
    }
}
