using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        //private readonly ISignInManager _signInManager;
        //private readonly IUserManager _userManager;
        //private readonly IRoleManager _roleManager;
        //private readonly ITokenService _tokenService;
        //private readonly ILogger<AccountController> _logger;

        //public CategoryController(
        //    IUserManager userManager,
        //    ISignInManager signInManager,
        //    IRoleManager roleManager,
        //    ITokenService tokenService,
        //    ILogger<AccountController> logger
        //    )
        //{
        //    _userManager = userManager;
        //    _signInManager = signInManager;
        //    _roleManager = roleManager;
        //    _tokenService = tokenService;
        //    _logger = logger;
        //}

        //[HttpGet]
        //public async Task<Category[]> GetAllCategories()
        //{
        //    return await _bookService.GetBook(id);
        //}
    }
}