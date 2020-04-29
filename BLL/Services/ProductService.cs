using BLL.DataAccess;
using BLL.Entities;
using BLL.Finders;
using BLL.Managers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class ProductService : IProductService
    {
        readonly IUnitOfWork _unitOfWork;
        readonly IRepository<Product> _repository;
        readonly IProductFinder _finder;

        public ProductService(IUnitOfWork unitOfWork, IRepository<Product> repository, IProductFinder finder)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
            _finder = finder;
        }

        public Product GetProduct(int id)
        {
            return _finder.GetById(id);
        }

        public IEnumerable<Product> GetAllProductClean()
        {
            return _finder.GetAllProductClean();
        }

        public IEnumerable<Product> GetVariantsByProductId(int productId)
        {
            return _finder.GetVariantsByProductId(productId);
        }

        public IEnumerable<Product> GetAll(string department, string category, string order, string range)
        {
            return _finder.GetAll(department, category, order, range);
        }

        public void Create(Product product)
        {
            if (product == null) return;
            _repository.Create(product);
            _unitOfWork.Commit();
        }

        public void Update(Product product)
        {
            if (product == null) return;
            _repository.Update(product);
            _unitOfWork.Commit();
        }

        public void Delete(Product product)
        {
            if (product == null) return;
            _repository.Delete(product);
            _unitOfWork.Commit();
        }
    }
}
