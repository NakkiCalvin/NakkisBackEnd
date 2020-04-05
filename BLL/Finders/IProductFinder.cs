using BLL.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Finders
{
    public interface IProductFinder
    {
        Product GetById(int id);
        IEnumerable<Product> GetAll();
        bool IsProductExists(Product product);
    }
}
