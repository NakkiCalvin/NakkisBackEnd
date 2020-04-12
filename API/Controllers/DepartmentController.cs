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
    [Route("api/departments")]
    [EnableCors("Policy")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        //private readonly ISignInManager _signInManager;
        //private readonly IUserManager _userManager;
        //private readonly IRoleManager _roleManager;
        //private readonly ITokenService _tokenService;
        //private readonly ILogger<AccountController> _logger;
        private readonly IDepartmentService _service;
        private readonly ICategoryService _categoryService;

        public DepartmentController(
        //    IUserManager userManager,
        //    ISignInManager signInManager,
        //    IRoleManager roleManager,
        //    ITokenService tokenService,
        //    ILogger<AccountController> logger
        IDepartmentService service,
        ICategoryService categoryService
            )
        {
            //    _userManager = userManager;
            //    _signInManager = signInManager;
            //    _roleManager = roleManager;
            //    _tokenService = tokenService;
            //    _logger = logger;
            _service = service;
            _categoryService = categoryService;
        }

        [HttpGet]
        public object GetAllDepartments()
        {
            var departments = _service.GetAll();
            var categories = _categoryService.GetAll();
            foreach (var entity in departments)
            {
                entity.Categories = categories;
            }
            var res = new { departments };
            return res;
        }
    }
}