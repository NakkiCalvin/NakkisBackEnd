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
    public class CartFinder : Finder<Cart>, ICartFinder
    {
        public CartFinder(DbSet<Cart> entity) : base(entity)
        {
        }

        public Cart GetCartByUserId(Guid userId)
        {
            return AsQueryable().Include(c => c.User).Include(y => y.CartItems).FirstOrDefault(x => x.UserId == userId);
        }

        public async Task<Cart> GetCartById(int cartId) {
            return await AsQueryable().Where(x => x.Id == cartId).FirstOrDefaultAsync();
        }
    }
}
