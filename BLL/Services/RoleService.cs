using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Entities;
using BLL.Managers;
using Microsoft.AspNetCore.Identity;

namespace BLL.Services
{
    public class RoleService : IRoleManager
    {
        private readonly RoleManager<Role> _manager;

        public RoleService(RoleManager<Role> manager)
        {
            _manager = manager;
        }

        public Task<IdentityResult> RemoveRole(Role role)
        {
            return _manager.DeleteAsync(role);
        }

        public List<Role> GetAllRolesSync()
        {
            return _manager.Roles.ToList();
        }

        public Task<bool> IsRoleExist(string role)
        {
            return _manager.RoleExistsAsync(role);
        }

        public Task<IdentityResult> AddRole(Role roleToAdd)
        {
            return _manager.CreateAsync(roleToAdd);
        }

    }
}
