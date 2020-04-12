using BLL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Managers
{
    public interface IVariantService
    {
        IEnumerable<Variant> GetAllById(int productId);
    }
}
