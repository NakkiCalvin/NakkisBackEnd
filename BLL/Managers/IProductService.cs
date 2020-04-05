using BLL.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Managers
{
    public interface IProductService
    {
        Product GetProduct(int id);
        IEnumerable<Product> GetAll();
        void Create(Product product);
        void Update(Product product);
        void Delete(Product product);
    }
}
