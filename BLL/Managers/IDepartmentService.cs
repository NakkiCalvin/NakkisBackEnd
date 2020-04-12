using BLL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Managers
{
    public interface IDepartmentService
    {
        IEnumerable<Department> GetAll();
    }
}
