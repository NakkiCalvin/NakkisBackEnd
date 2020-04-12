using BLL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Managers
{
    public interface ICartService
    {
        Cart GetCartByUserId(Guid userId);
        void Create(Cart cart);
        void Update(Cart cart);
    }
}
