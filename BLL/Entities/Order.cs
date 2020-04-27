using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public DateTimeOffset OrderDate { get; set; }
        public double TotalSum { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
    }
}
