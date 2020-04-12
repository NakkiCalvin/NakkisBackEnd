using BLL.Entities;
using BLL.Finders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Finder
{
    public class CartItemFinder : Finder<CartItem>, ICartItemFinder
    {
        public CartItemFinder(DbSet<CartItem> entity) : base(entity)
        {
        }

        public IEnumerable<CartItem> GetCartItemsByCartId(int cartId)
        {
            return AsQueryable().Include(y => y.Product).Where(x => x.CartId == cartId);
        }
    }
}
