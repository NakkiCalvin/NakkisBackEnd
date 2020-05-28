using BLL.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Managers
{
    public interface IOrderService
    {
        IEnumerable<Order> GetOrdersByUserId(Guid userId);
        void Create(Order cart);
        void Update(Order cart);
        Task<Order> GetOrderById(int orderId);
        Task<IEnumerable<Order>> GetAllOrders();
    }
}
