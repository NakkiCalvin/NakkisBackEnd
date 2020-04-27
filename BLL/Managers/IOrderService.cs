using BLL.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Managers
{
    public interface IOrderService
    {
        Order GetOrderByUserId(Guid userId);
        void Create(Order cart);
        void Update(Order cart);
        Task<Order> GetOrderById(int orderId);
    }
}
