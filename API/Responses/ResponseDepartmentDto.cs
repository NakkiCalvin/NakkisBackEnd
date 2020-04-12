using BLL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Responses
{
    public class ResponseDepartmentDto
    {
        public int Id { get; set; }
        public string DepartmentName { get; set; }
        //public string Categories { get; set; }
        public IEnumerable<Category> Categories { get; set; }
    }
}
