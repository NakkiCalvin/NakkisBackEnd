using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Responses;
using AutoMapper;
using BLL.Entities;
using BLL.Managers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/orders")]
    [EnableCors("Policy")]
    [ApiController]
    public class OwnerController : ControllerBase
    {
        //private readonly ISignInManager _signInManager;
        private readonly IUserManager _userManager;
        //private readonly IRoleManager _roleManager;
        //private readonly ITokenService _tokenService;
        //private readonly ILogger<AccountController> _logger;
        private readonly IProductService _service;
        private readonly IVariantService _variantService;
        private readonly ICartService _cartService;
        private readonly ICartItemsService _cartItemService;
        private readonly IOrderService _orderService;
        private readonly IOrderItemService _orderItemService;
        private readonly IAvailabilityService _availabilityService;

        public OwnerController(
            IUserManager userManager,
            //ISignInManager signInManager,
            //IRoleManager roleManager,
            //ITokenService tokenService,
            //ILogger<AccountController> logger
            IProductService service,
            IVariantService variantService,
            ICartService cartService,
            ICartItemsService cartItemService,
            IOrderService orderService,
            IOrderItemService orderItemService,
            IAvailabilityService availabilityService
            )
        {
            _service = service;
            _cartService = cartService;
            _cartItemService = cartItemService;
            _variantService = variantService;
            _orderItemService = orderItemService;
            _orderService = orderService;
            _userManager = userManager;
            _availabilityService = availabilityService;
            //_signInManager = signInManager;
            //_roleManager = roleManager;
            //_tokenService = tokenService;
            //_logger = logger;
        }

        [HttpGet("avale")]
        [Authorize(Roles = "Admin")]
        public async Task<object> GetAllAvale()
        {
            var entities = await _availabilityService.GetAvailabilites();
            var avale = Mapper.Map<IEnumerable<AvalabilityModel>>(entities);
            return avale;
        }

        [HttpPost("sendAvale")]
        [Authorize(Roles = "Admin")]
        public async Task<object> sendAvale(AvalabilityModel model)
        {
            var item = _availabilityService.GetAvailabilityById(model.Id);
            item.Quantity = model.Quantity;
            _availabilityService.Update(item);
            var entities = await _availabilityService.GetAvailabilites();
            var avale = Mapper.Map<IEnumerable<AvalabilityModel>>(entities);
            return avale;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<object> GetAllOrders()
        {
            var entities = await _orderService.GetAllOrders();
            //List<Order> lst = new List<Order>();
            var data = entities.Select(k => new { k.OrderDate.Year, k.OrderDate.Month, k.TotalSum }).GroupBy(x => new { x.Year, x.Month }, (key, group) => new
            {
                yr = key.Year,
                mnth = key.Month,
                tSum = group.Sum(k => k.TotalSum)
            }).OrderBy(x => x.mnth).ToList();

            return data;
        }
    }
}