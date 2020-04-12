﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BLL.Entities
{
    public class Department
    {
        public int Id { get; set; }
        public string DepartmentName { get; set; }
        //public string Categories { get; set; }
        [NotMapped]
        public IEnumerable<Category> Categories { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
