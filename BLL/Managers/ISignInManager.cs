using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BLL.Entities;
using Microsoft.AspNetCore.Identity;

namespace BLL.Managers
{
    public interface ISignInManager
    {
        Task<SignInResult> CheckPass(User user, string pass, bool lockoutFail);
        Task Logout();
        Task<SignInResult> SignIn(User user, string pass);
    }
}
