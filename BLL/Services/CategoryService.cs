using BLL.DataAccess;
using BLL.Entities;
using BLL.Finders;
using BLL.Managers;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Services
{
    public class CategoryService : ICategoryService
    {
        readonly IUnitOfWork _unitOfWork;
        readonly IRepository<Category> _repository;
        readonly ICategoryFinder _finder;

        public CategoryService(IUnitOfWork unitOfWork, IRepository<Category> repository, ICategoryFinder finder)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
            _finder = finder;
        }

        public IEnumerable<Category> GetAll()
        {
            return _finder.GetAll();
        }
    }
}
