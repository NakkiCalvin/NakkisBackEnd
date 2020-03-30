using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Entities;

namespace API.Requests
{
    public class RegisterUserModel
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public RegisterUserModel() { }

        public static explicit operator User(RegisterUserModel model) => new User { Email = model.Email, UserName = model.Email };
    }
}
