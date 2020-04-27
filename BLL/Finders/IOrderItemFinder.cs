using BLL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Finders
{
    public interface IOrderItemFinder
    {
        IEnumerable<OrderItem> GetOrderItemsByOrderId(int orderId);
    }
}
