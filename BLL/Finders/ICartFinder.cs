﻿using BLL.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Finders
{
    public interface ICartFinder
    {
        Cart GetCartByUserId(Guid userId);

        Task<Cart> GetCartById(int cartId);
    }
}
