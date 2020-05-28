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

        public IEnumerable<Order> GetOrdersByUserId(Guid userId)
        {
            return AsQueryable().Include(c => c.User).Include(y => y.OrderItems).Where(x => x.UserId == userId).ToList();
        }

        public async Task<Order> GetOrderById(int orderId)
        {
            return await AsQueryable().Where(x => x.Id == orderId).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Order>> GetAllOrders()
        {
            return await AsQueryable().ToListAsync();
        }
    }
}
