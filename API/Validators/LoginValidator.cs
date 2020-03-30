using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Requests;
using BLL.Managers;
using FluentValidation;

namespace API.Validators
{
    public class LoginValidator : AbstractValidator<LoginModel>
    {
        public LoginValidator(IUserManager userManager, ISignInManager signInManager)
        {
            RuleFor(x => x.Email).EmailAddress().NotEmpty().MustAsync(async (model, email, context) =>
            {
                var userResult = await userManager.GetUserByEmail(email);
                return userResult != null;
            }).WithMessage($"Invalid Email");

            RuleFor(x => x.Password).NotEmpty().WithMessage($"Password can't be empty")
                .MustAsync(async (model, email, context) =>
                {
                    var user = await userManager.GetUserByEmail(model.Email);
                    var passwordResult = await signInManager.CheckPass(user, model.Password, false);
                    return passwordResult.Succeeded;
                }).WithMessage($"Incorrect password");
        }
    }
}
