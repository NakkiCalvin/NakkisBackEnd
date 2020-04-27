using BLL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Managers
{
    public interface ICartItemsService
    {
        IEnumerable<CartItem> GetCartItemsByCartId(int cartId);
        void Create(CartItem cartItem);
        void Update(CartItem cartItem);
        void Delete(CartItem cartItem);
        void Delete(IEnumerable<CartItem> cartItems);
    }
}
