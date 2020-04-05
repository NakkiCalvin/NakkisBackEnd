using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Entities
{
    public class Department
    {
        public int Id { get; set; }
        public string DepartmentName { get; set; }
        //public string Categories { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
