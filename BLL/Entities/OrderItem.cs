﻿using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Entities
{
    public class OrderItem
    {
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public int AvailabilityId { get; set; }
        public Availability Availability { get; set; }
        //public int ProductId { get; set; }
        //public Product Product { get; set; }
        public int Qty { get; set; }
        public double Price { get; set; }
    }
}
