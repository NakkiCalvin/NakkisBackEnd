using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Requests;
using API.Responses;
using AutoMapper;
using BLL.Entities;
using BLL.Managers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
    [Route("api/account")]
    [EnableCors("Policy")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ISignInManager  _signInManager;
        private readonly IUserManager _userManager;
        private readonly IRoleManager _roleManager;
        private readonly ITokenService _tokenService;
        private readonly ICartService _cartService;
        private readonly ICartItemsService _cartItemService;
        private readonly IProductService _productService;
        private readonly IAvailabilityService _availabilityService;
        private readonly IOrderService _orderService;
        private readonly ILogger<AccountController> _logger;

        public AccountController(
            IUserManager userManager,
            ISignInManager signInManager,
            IRoleManager roleManager,
            ITokenService tokenService,
            ICartService cartService,
            ICartItemsService cartItemService,
            IOrderService orderService,
            IProductService productService,
            IAvailabilityService availabilityService,
            ILogger<AccountController> logger
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _tokenService = tokenService;
            _cartService = cartService;
            _cartItemService = cartItemService;
            _orderService = orderService;
            _productService = productService;
            _availabilityService = availabilityService;
            _logger = logger;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            var identity = (ClaimsIdentity)this.User.Identity;
            //var userEmail = identity.FindFirst(JwtRegisteredClaimNames.Sub).Value;
            //var user = await _userManager.GetUserByEmail(userEmail);
            await _signInManager.Logout();
            _logger.LogTrace("User logged out");
            return Ok();
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterUserModel model)
        {
            _logger.LogTrace($"{model.Email}, registration processing...");

            if (model.Password != model.verifyPassword)
            {
                return BadRequest("Passwort didn't match the original");
            }

            var mapUser = Mapper.Map<RegisterUserModel, User>(model);
            mapUser.Id = Guid.NewGuid();

            await _userManager.CreateUser(mapUser, model.Password);
            await _userManager.AddToRole(mapUser, "User");

            _logger.LogTrace($"Registration finished Successfully. EMAIL: {model.Email}, PASS: {model.Password} ");

            return Ok(mapUser);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<object> GenerateToken(LoginModel authorize)
        {
            _logger.LogTrace($"{authorize.Email} started to logging in...");

            var user = await _userManager.GetUserByEmail(authorize.Email);
            var userRoles = await _userManager.GetUserRoles(user);

            var configuredToken = new
            {
                user_token = new {
                    user_id = user.Id, user_name = user.UserName,
                    token = _tokenService.GetEncodedJwtToken(userRoles, authorize.Email),
                    roles = userRoles }
            };
            _logger.LogTrace($"{authorize.Email} successfully logging in...");
            return configuredToken;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("{userId}/orders")]
        public object GetUserOrders([FromRoute]Guid userId)
        {
            var orders = _orderService.GetOrdersByUserId(userId);
            var userOrderList = new List<ResponseUserOrder>();
            foreach (var order in orders)
            {
                foreach(var item in order.OrderItems)
                {
                    var avale = _availabilityService.GetAvailabilityById(item.AvailabilityId);
                    userOrderList.Add(new ResponseUserOrder()
                    {
                        Id = order.Id, Color = avale.Variant.Color, Description = avale.Variant.Product.Description, OrderDateTime = order.OrderDate.ToString("MM/dd/yyyy HH:mm"),
                        Price = item.Price, Quantity = item.Qty, Size = avale.Size.ToString(), Title = avale.Variant.Product.Title, TotalSum = order.TotalSum,
                    });
                }
            }
            return userOrderList;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("{userId}/cart")]
        public object GetUserCart([FromRoute]Guid userId)
        {
            return mapCartResponse(userId);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("{userId}/cart")]
        public object PostUserCart(RequestCart model)
        {
            var cart = _cartService.GetCartByUserId(model.UserId);
            var availability = _availabilityService.GetAvailabilityByVariantId(model.VariantId, int.Parse(model.size));

            if (cart == null)
            {
                _cartService.Create(new Cart { UserId = model.UserId });
                var newCart = _cartService.GetCartByUserId(model.UserId);
                _cartItemService.Create(new CartItem { CartId = newCart.Id, AvailabilityId = availability.Id, Price = availability.Variant.Product.Price, Qty = 1, FirstSeenDate = DateTimeOffset.Now });
                availability.Quantity = availability.Quantity - 1;
                _availabilityService.Update(availability);
                newCart.TotalPrice = availability.Variant.Product.Price;
                newCart.TotalQty = 1;
                _cartService.Update(newCart);

                return mapCartResponse(model.UserId);
            }
            var entitiesCartItems = _cartItemService.GetCartItemsByCartId(cart.Id);
            var cartItem = entitiesCartItems.Where(_ => _.AvailabilityId == availability.Id).FirstOrDefault();
            if (cartItem == null)
            {
                _cartItemService.Create(new CartItem { CartId = cart.Id, AvailabilityId = availability.Id, Price = availability.Variant.Product.Price, Qty = 1, FirstSeenDate = DateTimeOffset.Now });
                availability.Quantity = availability.Quantity - 1;
                _availabilityService.Update(availability);
                var newCartItems = _cartItemService.GetCartItemsByCartId(cart.Id);
                cart.TotalPrice = Math.Round(newCartItems.Sum(_ => _.Price), 2);
                cart.TotalQty = newCartItems.Sum(_ => _.Qty);
                _cartService.Update(cart);

                return mapCartResponse(model.UserId);
            }
            if (model.increase)
            {
                cartItem.Price = Math.Round(cartItem.Price + availability.Variant.Product.Price, 2);
                cartItem.Qty = cartItem.Qty + 1;
                _cartItemService.Update(cartItem);
                availability.Quantity = availability.Quantity - 1;
                _availabilityService.Update(availability);

                var newCartItems = _cartItemService.GetCartItemsByCartId(cart.Id);
                cart.TotalPrice = Math.Round(newCartItems.Sum(_ => _.Price), 2);
                cart.TotalQty = newCartItems.Sum(_ => _.Qty);
                _cartService.Update(cart);

                return mapCartResponse(model.UserId);
            }
            if (model.decrease)
            {
                if (cartItem.Qty == 1)
                {
                    _cartItemService.Delete(cartItem);
                    availability.Quantity = availability.Quantity + 1;
                    _availabilityService.Update(availability);

                    var newCartItems = _cartItemService.GetCartItemsByCartId(cart.Id);
                    if (newCartItems == null)
                    {
                        cart.TotalPrice = 0;
                        cart.TotalQty = 0;
                        _cartService.Update(cart);
                    }
                    else
                    {
                        cart.TotalPrice = Math.Round(newCartItems.Sum(_ => _.Price), 2);
                        cart.TotalQty = newCartItems.Sum(_ => _.Qty);
                        _cartService.Update(cart);
                    }
                }
                else
                {
                    cartItem.Price = Math.Round(cartItem.Price - availability.Variant.Product.Price, 2);
                    cartItem.Qty = cartItem.Qty - 1;
                    _cartItemService.Update(cartItem);

                    availability.Quantity = availability.Quantity + 1;
                    _availabilityService.Update(availability);

                    var newCartItems = _cartItemService.GetCartItemsByCartId(cart.Id);
                    cart.TotalPrice = Math.Round(newCartItems.Sum(_ => _.Price), 2);
                    cart.TotalQty = newCartItems.Sum(_ => _.Qty);
                    _cartService.Update(cart);
                }
                return mapCartResponse(model.UserId);
            }

            return BadRequest("Error");
        }

        private object mapCartResponse(Guid userId)
        {
            var userCart = _cartService.GetCartByUserId(userId);
            var cartItmes = _cartItemService.GetCartItemsByCartId(userCart.Id);
            var dtoCart = Mapper.Map<Cart, CartModel>(userCart);
            var dtoCartItems = Mapper.Map<IEnumerable<CartItem>, IEnumerable<CartItemDto>>(cartItmes);

            dtoCart.Items = dtoCartItems;
            return new { cart = dtoCart };
        } 

        [HttpPost("createRole")]
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
