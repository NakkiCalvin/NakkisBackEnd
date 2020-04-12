using BLL.DataAccess;
using BLL.Entities;
using BLL.Finders;
using BLL.Managers;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Services
{
    public class VariantService : IVariantService
    {
        readonly IUnitOfWork _unitOfWork;
        readonly IRepository<Variant> _repository;
        readonly IVariantFinder _finder;

        public VariantService(IUnitOfWork unitOfWork, IRepository<Variant> repository, IVariantFinder finder)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
            _finder = finder;
        }

        public IEnumerable<Variant> GetAllById(int productId)
        {
            return _finder.GetAllById(productId);
        }
    }
}
