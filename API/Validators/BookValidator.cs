using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Requests;
using BLL.Managers;
using FluentValidation;

namespace API.Validators
{
    public class BookValidator : AbstractValidator<RequestBookModel>
    {
        public BookValidator(IBookService bookService)
        {
            RuleFor(x => x.Title).NotEmpty().NotNull().MinimumLength(1).MaximumLength(50);
            RuleFor(x => x.Content).NotEmpty().NotNull().MinimumLength(5).MaximumLength(500);
        }
    }
}
