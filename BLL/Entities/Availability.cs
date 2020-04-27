using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Entities
{
    public class Availability
    {
        public int Id { get; set; }
        public int Size { get; set; }
        public int Quantity { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        //public int VariantId { get; set; }
        //public Variant Variant { get; set; }
    }
}
