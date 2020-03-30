using BLL.Entities;

namespace API.Requests
{
    public class LoginModel
    {
        public LoginModel() { }
        public string Email { get; set; }
        public string Password { get; set; }

        public static explicit operator User(LoginModel userModel)
        {
            return new User
            {
                Email = userModel.Email,
                UserName = userModel.Email
            };
        }
    }
}
