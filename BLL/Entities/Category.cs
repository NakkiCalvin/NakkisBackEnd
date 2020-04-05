using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        //public int DepartmentId { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
