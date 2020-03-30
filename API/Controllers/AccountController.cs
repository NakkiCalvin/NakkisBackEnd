using System;
using System.Threading.Tasks;
using API.Requests;
using AutoMapper;
using BLL.Entities;
using BLL.Managers;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
    [Route("Account")]
    [EnableCors("Policy")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ISignInManager  _signInManager;
        private readonly IUserManager _userManager;
        private readonly IRoleManager _roleManager;
        private readonly ITokenService _tokenService;
        private readonly ILogger<AccountController> _logger;

        public AccountController(
            IUserManager userManager,
            ISignInManager signInManager,
            IRoleManager roleManager,
            ITokenService tokenService,
            ILogger<AccountController> logger
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _tokenService = tokenService;
            _logger = logger;
        }

        [HttpGet]
        [Route("LogOut")]
        public async Task Logout()
        {
            await _signInManager.Logout();
            _logger.LogTrace("User logged out");
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(RegisterUserModel model)
        {
            _logger.LogTrace($"{model.Email}, registration processing...");

            var mapUser = Mapper.Map<RegisterUserModel, User>(model);
            mapUser.Id = Guid.NewGuid();

            await _userManager.CreateUser(mapUser, model.Password);
            await _userManager.AddToRole(mapUser, "User");

            _logger.LogTrace($"Registration finished Successfully. EMAIL: {model.Email}, PASS: {model.Password} ");

            return Ok(mapUser);
        }

        [HttpPost]
        [Route("Login")]
        public object GenerateToken(LoginModel authorize)
        {
            _logger.LogTrace($"{authorize.Email} started to logging in...");
            var configuredToken = new
            {
                access_token = _tokenService.GetEncodedJwtToken(authorize.Email),
                userEmail = authorize.Email,
            };
            _logger.LogTrace($"{authorize.Email} successfully logging in...");
            return configuredToken;
        }

        [HttpPost]
        [Route("CreateRole")]
        public async Task<RoleModel> Create(RoleModel roleModel)
        {
            _logger.LogTrace($"Creating {roleModel.Name} role...");
            roleModel.Id = Guid.NewGuid();

            await _roleManager.AddRole((Role)roleModel);

            _logger.LogTrace($"{roleModel.Name} was successfully created.");
            return roleModel;
        }
    }
}
