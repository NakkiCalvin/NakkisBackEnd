using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Entities;

namespace API.Requests
{
    public class RoleModel
    {
        public string Name { get; set; }
        public Guid Id { get; set; }

        public RoleModel()
        {

        }
        public static explicit operator Role(RoleModel role)
        {
            return new Role()
            {
                Id = role.Id,
                Name = role.Name
            };
        }
    }
}
