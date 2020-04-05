using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Entities
{
    public class Cart
    {
        public int Id { get; set; }
        public int TotalQty { get; set; }
        public double TotalPrice { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }

        public ICollection<CartItem> CartItems { get; set; }
    }
}
