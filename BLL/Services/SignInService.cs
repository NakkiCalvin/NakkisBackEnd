using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BLL.Entities;
using BLL.Managers;
using Microsoft.AspNetCore.Identity;

namespace BLL.Services
{
    public class SignInService : ISignInManager
    {
        private readonly SignInManager<User> _manager;

        public SignInService(SignInManager<User> manager)
        {
            _manager = manager;
        }

        public async Task<SignInResult> CheckPass(User user, string pass, bool lockoutFail)
        {
            return await _manager.CheckPasswordSignInAsync(user, pass, lockoutFail);
        }

        public async Task Logout()
        {
            await _manager.SignOutAsync();
        }

        public Task<SignInResult> SignIn(User user, string pass)
        {
            throw new NotImplementedException();
        }
    }
}
