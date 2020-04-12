using BLL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Finders
{
    public interface IDepartmentFinder
    {
        IEnumerable<Department> GetAll();
    }
}
