using BLL.Entities;
using BLL.Finders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Finder
{
    public class DepartmentFinder : Finder<Department>, IDepartmentFinder
    {
        public DepartmentFinder(DbSet<Department> entity) : base(entity)
        {
        }

        public IEnumerable<Department> GetAll()
        {
            return AsQueryable().ToList();
        }
    }
}
