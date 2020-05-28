using BLL.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Finders
{
    public interface IOrderFinder
    {
        IEnumerable<Order> GetOrdersByUserId(Guid userId);
        Task<Order> GetOrderById(int orderId);
        Task<IEnumerable<Order>> GetAllOrders();
    }
}
