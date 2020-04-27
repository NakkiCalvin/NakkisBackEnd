using BLL.Entities;
using BLL.Finders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Finder
{
    public class OrderFinder : Finder<Order>, IOrderFinder
    {
        public OrderFinder(DbSet<Order> entity) : base(entity)
        {
        }

        public Order GetOrderByUserId(Guid userId)
        {
            return AsQueryable().Include(c => c.User).Include(y => y.OrderItems).FirstOrDefault(x => x.UserId == userId);
        }

        public async Task<Order> GetOrderById(int orderId)
        {
            return await AsQueryable().Where(x => x.Id == orderId).FirstOrDefaultAsync();
        }
    }
}
