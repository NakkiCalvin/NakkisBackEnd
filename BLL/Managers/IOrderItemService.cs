using BLL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Managers
{
    public interface IOrderItemService
    {
        IEnumerable<OrderItem> GetOrderItemsByOrderId(int orderId);
        void Create(OrderItem orderItem);
        void Update(OrderItem orderItem);
        void Delete(OrderItem orderItem);
    }
}
