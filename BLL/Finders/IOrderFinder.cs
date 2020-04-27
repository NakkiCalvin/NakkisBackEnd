using BLL.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Finders
{
    public interface IOrderFinder
    {
        Order GetOrderByUserId(Guid userId);
        Task<Order> GetOrderById(int orderId);
    }
}
