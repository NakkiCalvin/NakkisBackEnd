using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.Entities;
using Microsoft.AspNetCore.Identity;

namespace BLL.Managers
{
    public interface IRoleManager
    {
        Task<IdentityResult> AddRole(Role roleToAdd);
        Task<IdentityResult> RemoveRole(Role role);
        List<Role> GetAllRolesSync();
        Task<bool> IsRoleExist(string role);

    }
}