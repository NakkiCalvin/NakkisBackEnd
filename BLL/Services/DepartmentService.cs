using BLL.DataAccess;
using BLL.Entities;
using BLL.Finders;
using BLL.Managers;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Services
{
    public class DepartmentService : IDepartmentService
    {
        //readonly IUnitOfWork _unitOfWork;
        //readonly IRepository<Department> _repository;
        readonly IDepartmentFinder _finder;

        public DepartmentService(
            //IUnitOfWork unitOfWork,
            //IRepository<Department> repository, 
            IDepartmentFinder finder)
        {
            //_unitOfWork = unitOfWork;
            //_repository = repository;
            _finder = finder;
        }

        public IEnumerable<Department> GetAll()
        {
            return _finder.GetAll();
        }
    }
}
