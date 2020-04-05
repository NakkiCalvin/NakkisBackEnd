using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Entities;
using BLL.Managers;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/products")]
    [EnableCors("Policy")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        //private readonly ISignInManager _signInManager;
        //private readonly IUserManager _userManager;
        //private readonly IRoleManager _roleManager;
        //private readonly ITokenService _tokenService;
        //private readonly ILogger<AccountController> _logger;
        private readonly IProductService _service;

        public ProductController(
            //IUserManager userManager,
            //ISignInManager signInManager,
            //IRoleManager roleManager,
            //ITokenService tokenService,
            //ILogger<AccountController> logger
            IProductService service
            )
        {
            _service = service;
            //_userManager = userManager;
            //_signInManager = signInManager;
            //_roleManager = roleManager;
            //_tokenService = tokenService;
            //_logger = logger;
        }

        [HttpGet]
        public object GetAllProducts([FromQuery] string department, [FromQuery] string category, [FromQuery] string order, [FromQuery] string range)
        {
            var products = _service.GetAll();
            var res = new { products };
            return res;
        }

        [HttpGet("{productID}")]
        public object GetProduct([FromRoute] int productId)
        {
            var product = _service.GetProduct(productId);
            var res = new { product };
            return res;
            //var user = await GetActualUser();
            //_logger.LogTrace($"Getting {user.Email} books...");
            //IEnumerable<Book> userBookList = _bookService.GetAll(user.Id);
            //Book[] books = userBookList.ToArray();
            //_logger.LogTrace($"{user.Email} books was successfully found");
            //return books;
        }
    }
}