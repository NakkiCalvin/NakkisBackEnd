using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
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
        private readonly ILogger<AccountController> _logger;

        public AccountController(
            IUserManager userManager,
            ISignInManager signInManager,
            IRoleManager roleManager,
            ITokenService tokenService,
            ICartService cartService,
            ICartItemsService cartItemService,
            IProductService productService,
            ILogger<AccountController> logger
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _tokenService = tokenService;
            _cartService = cartService;
            _cartItemService = cartItemService;
            _productService = productService;
            _logger = logger;
        }

        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.Logout();
            var identity = (ClaimsIdentity)this.User.Identity;
            var userEmail = identity.FindFirst(JwtRegisteredClaimNames.Sub).Value;
            var user = await _userManager.GetUserByEmail(userEmail);
            _logger.LogTrace("User logged out");
            return Ok();
        }

        [HttpPost("register")]
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

        [HttpPost("login")]
        public async Task<object> GenerateToken(LoginModel authorize)
        {
            _logger.LogTrace($"{authorize.Email} started to logging in...");

            var user = await _userManager.GetUserByEmail(authorize.Email);
            
            var configuredToken = new
            {
                user_token = new { user_id = user.Id, user_name = user.UserName, token = _tokenService.GetEncodedJwtToken(authorize.Email) }
            };
            _logger.LogTrace($"{authorize.Email} successfully logging in...");
            return configuredToken;
        }

        [HttpGet("{userId}/cart")]
        public object GetUserCart([FromRoute]Guid userId)
        {
            var cart = _cartService.GetCartByUserId(userId);
            var cartItmes = _cartItemService.GetCartItemsByCartId(cart.Id);
            var dtoCart = Mapper.Map<Cart, CartModel>(cart);
            var dtoCartItems = Mapper.Map<IEnumerable<CartItem>, IEnumerable<CartItemDto>>(cartItmes);
            foreach (var item in dtoCartItems)
            {
                item.Item.CartItems = null;
            }

            dtoCart.Items = dtoCartItems;
            var res = new { cart = dtoCart };
            return res;
        }

        [HttpPost("{userId}/cart")]
        public object PostUserCart(RequestCart model)
        {
            var cart = _cartService.GetCartByUserId(model.UserId);
            var product = _productService.GetProduct(model.ProductId);

            if (cart == null)
            {
                _cartService.Create(new Cart { UserId = model.UserId });
                var newCart = _cartService.GetCartByUserId(model.UserId);
                _cartItemService.Create(new CartItem { CartId = newCart.Id, ProductId = product.Id, Price = product.Price, Qty = 1 });
                newCart.TotalPrice = product.Price;
                newCart.TotalQty = 1;
                _cartService.Update(newCart);

                return mapCartResponse(model.UserId);
            }
            var entitiesCartItems = _cartItemService.GetCartItemsByCartId(cart.Id);
            var cartItem = entitiesCartItems.Where(_ => _.ProductId == model.ProductId).FirstOrDefault();
            if (cartItem == null)
            {
                _cartItemService.Create(new CartItem { CartId = cart.Id, ProductId = product.Id, Price = product.Price, Qty = 1 });

                var newCartItems = _cartItemService.GetCartItemsByCartId(cart.Id);
                cart.TotalPrice = Math.Round(newCartItems.Sum(_ => _.Price), 2);
                cart.TotalQty = newCartItems.Sum(_ => _.Qty);
                _cartService.Update(cart);

                return mapCartResponse(model.UserId);
            }
            if (model.increase)
            {
                cartItem.Price = Math.Round(cartItem.Price + product.Price, 2);
                cartItem.Qty = cartItem.Qty + 1;
                _cartItemService.Update(cartItem);

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
                    cartItem.Price = Math.Round(cartItem.Price - product.Price, 2);
                    cartItem.Qty = cartItem.Qty - 1;
                    _cartItemService.Update(cartItem);

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
            foreach (var item in dtoCartItems)
            {
                item.Item.CartItems = null;
                item.Item.Category = null;
                item.Item.Department = null;
            }

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
