using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Entities
{
    public class Variant
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public string ImagePath { get; set; }
        public string Color { get; set; }
        public ICollection<Availability> Availabilities { get; set; }
        //public string Size { get; set; }
        //public int Quantity { get; set; }
    }
}
