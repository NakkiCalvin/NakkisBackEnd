using BLL.Entities;
using BLL.Finders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Finder
{
    public class OrderItemFinder : Finder<OrderItem>, IOrderItemFinder
    {
        public OrderItemFinder(DbSet<OrderItem> entity) : base(entity)
        {
        }

        public IEnumerable<OrderItem> GetOrderItemsByOrderId(int orderId)
        {
            return AsQueryable().Include(y => y.Product).Where(x => x.OrderId == orderId);
        }
    }
}
