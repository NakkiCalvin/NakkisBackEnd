using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using API.PayPal;
using API.Requests;
using API.Responses;
using AutoMapper;
using BLL.Entities;
using BLL.Managers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
//using PayPal.Api;
using PayPalCheckoutSdk.Orders;
using Order = PayPalCheckoutSdk.Orders.Order;
//using PayPalCheckoutSdk.Payments;

namespace API.Controllers
{
    [Route("api/products")]
    [EnableCors("Policy")]
    [ApiController]
    public class ProductController : ControllerBase
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

        public ProductController(
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
            _availabilityService = availabilityService;
            _cartService = cartService;
            _cartItemService = cartItemService;
            _variantService = variantService;
            _orderItemService = orderItemService;
            _orderService = orderService;
            _userManager = userManager;
            //_signInManager = signInManager;
            //_roleManager = roleManager;
            //_tokenService = tokenService;
            //_logger = logger;
        }

        [HttpGet("search")]
        public object GetAllProducts([FromQuery] string query)
        {
            if (query != null)
            {
                var entities = _service.GetAll(null, null, null, null).Where(x => x.Title == query || x.Category.CategoryName == query || x.Department.DepartmentName == query);
                var products = Mapper.Map<IEnumerable<ResponseProductModel>>(entities);
                foreach (var entity in products)
                {
                    var original = _variantService.GetAllById(entity.Id).FirstOrDefault(x => x.Color == "Original");
                    entity.Color = original.Color;
                    entity.VariantSizes = original.Availabilities.Select(x => x.Size.ToString());
                }
                var res = new { products };
                return res;
            }
            else
            {
                var entities = _service.GetAll(null, null, null, null);
                var products = Mapper.Map<IEnumerable<ResponseProductModel>>(entities);
                foreach (var entity in products)
                {
                    var original = _variantService.GetAllById(entity.Id).FirstOrDefault(x => x.Color == "Original");
                    entity.Color = original.Color;
                    entity.VariantSizes = original.Availabilities.Select(x => x.Size.ToString());
                }
                var res = new { products };
                return res;
            }
        }

        [HttpGet]
        public async Task<object> GetAllProducts([FromQuery] string department, [FromQuery] string category, [FromQuery] string order, [FromQuery] string range, [FromQuery] string size)
        {
            var entities = _service.GetAll(department, category, order, range);

            if (size != null)
            {
                var avale = await _availabilityService.GetAvailabilites();
                var idArray = avale.Where(x => x.Size == int.Parse(size) && x.Variant.Color == "Original").Select(y => y.Variant.ProductId);
                var productFilteredBySize = new List<Product>();
                foreach (var entity in entities)
                {
                    foreach (var id in idArray)
                    {
                        if (entity.Id == id)
                        {
                            productFilteredBySize.Add(entity);
                        }
                    }
                }
                entities = productFilteredBySize;
            }
            
            var products = Mapper.Map<IEnumerable<ResponseProductModel>>(entities);
            foreach (var entity in products)
            {
                var original = _variantService.GetAllById(entity.Id).FirstOrDefault(x => x.Color == "Original");
                entity.Color = original.Color;
                entity.VariantSizes = original.Availabilities.Select(x => x.Size.ToString());
            }
            var res = new { products };
            return res;
        }

        [HttpGet("{productID}")]
        public object GetProduct([FromRoute] int productId)
        {
            var entity = _service.GetProduct(productId);
            var product = Mapper.Map<Product, ResponseProductModel>(entity);
            var original = _variantService.GetAllById(entity.Id).FirstOrDefault(x => x.Color == "Original");
            product.VariantId = original.Id;
            product.Color = original.Color;
            product.VariantSizes = original.Availabilities.Select(x => x.Size.ToString());
            var sizeDictionary = new Dictionary<string, int>();
            foreach (var item in original.Availabilities)
            {
                sizeDictionary.Add(item.Size.ToString(), item.Quantity);
            }
            product.QuantitySizes = sizeDictionary;
            var res = new { product };
            return res;
        }

        [HttpGet("variants")]
        public object GetAllProductVariants([FromQuery] int productId)
        {
            var entities = _variantService.GetAllById(productId).Where(x => x.Color != "Original");

            var variants = new List<ResponseProductModel>();
            foreach (var variant in entities)
            {
                var prod = Mapper.Map<Product, ResponseProductModel>(variant.Product);
                prod.VariantId = variant.Id;
                prod.Color = variant.Color;
                prod.ImagePath = variant.ImagePath;
                prod.VariantSizes = variant.Availabilities.Select(x => x.Size.ToString());
                var sizeDictionary = new Dictionary<string, int>();
                foreach (var item in variant.Availabilities)
                {
                    sizeDictionary.Add(item.Size.ToString(), item.Quantity);
                }
                prod.QuantitySizes = sizeDictionary;
                variants.Add(prod);
            }
            var res = new { variants };
            return res;
        }

        [HttpGet("payment/success")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<object> VerifyOrder([FromQuery] string orderId)
        {
            // Construct a request object and set desired parameters
            // Replace ORDER-ID with the approved order id from create order
            //var request = new OrdersCaptureRequest(orderId);
            //request.RequestBody(new OrderActionRequest());
            //var response = await CaptureOrderSample.client().Execute(request);
            //var statusCode = response.StatusCode;
            //Order result = response.Result<Order>();
            //Console.WriteLine("Status: {0}", result.Status);
            //Console.WriteLine("Capture Id: {0}", result.Id);
            //return result;


            ////
            ///

            OrdersGetRequest request = new OrdersGetRequest(orderId);
            //3. Call PayPal to get the transaction
            var response = await CaptureOrderSample.client().Execute(request);
            //4. Save the transaction in your database. Implement logic to save transaction to your database for future reference.
            var result = response.Result<Order>();
            Console.WriteLine("Retrieved Order Status");
            Console.WriteLine("Status: {0}", result.Status);
            Console.WriteLine("Order Id: {0}", result.Id);
            Console.WriteLine("Links:");
            foreach (LinkDescription link in result.Links)
            {
                Console.WriteLine("\t{0}: {1}\tCall Type: {2}", link.Rel, link.Href, link.Method);
            }

            if (result.Status == "APPROVED")
            {
                var identity = (ClaimsIdentity)this.User.Identity;
                var userEmail = identity.FindFirst(JwtRegisteredClaimNames.Sub).Value;
                var user = await _userManager.GetUserByEmail(userEmail);

                var userCart = _cartService.GetCartByUserId(user.Id);
                var order = new BLL.Entities.Order { OrderDate = DateTimeOffset.Now, UserId = user.Id, TotalSum = userCart.TotalPrice };
                _orderService.Create(order);

                foreach (var item in userCart.CartItems)
                {
                    var orderitem = new OrderItem { OrderId = order.Id, AvailabilityId = item.AvailabilityId, Price = item.Price, Qty = item.Qty };
                    _orderItemService.Create(orderitem);
                }

                _cartItemService.Delete(userCart.CartItems);

                userCart.CartItems = null;
                userCart.TotalPrice = 0;
                userCart.TotalQty = 0;
                _cartService.Update(userCart);
            }

            AmountWithBreakdown amount = result.PurchaseUnits[0].AmountWithBreakdown;
            Console.WriteLine("Total Amount: {0} {1}", amount.CurrencyCode, amount.Value);
            var addr = result.PurchaseUnits.FirstOrDefault().ShippingDetail.AddressPortable;
            var entitiy = new { cart = result.Id, shippingDetails = new {
                addressLine1 = addr.AddressLine1,
                addressLine2 = addr.AddressLine2,
                adminArea1 = addr.AdminArea1,
                countryCode = addr.CountryCode } };
            var res = new { payment = entitiy };
            return res;
        }

        [HttpGet("checkout/{cartId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<object> createOrder([FromRoute] int cartId)
        {
            PayPalHttp.HttpResponse response;
            var currenctCart = await _cartService.GetCartById(cartId);
            var cartItems = _cartItemService.GetCartItemsByCartId(cartId).ToList();
            var items = new List<Item>();
            foreach (var item in cartItems)
            {
                items.Add(
                    new Item
                    {
                        Name = item.Availability.Variant.Product.Title,
                        Description = item.Availability.Variant.Product.Description.Substring(0, 10),
                        Sku = "sku01",
                        UnitAmount = new Money
                        {
                            CurrencyCode = "USD",
                            Value = Math.Round(item.Availability.Variant.Product.Price, 2).ToString(CultureInfo.InvariantCulture),
                        },
                        Tax = new Money
                        {
                            CurrencyCode = "USD",
                            Value = Math.Round(item.Availability.Variant.Product.Price * 0.13, 2).ToString(CultureInfo.InvariantCulture)
                        },
                        Quantity = item.Qty.ToString(),
                        //Category = item.Product.Category.CategoryName,
                    });
            }

            //var payer = new Payer() { payment_method = "paypal" };

            //var guid = Convert.ToString((new Random()).Next(100000));
            //var redirUrls = new RedirectUrls()
            //{
            //    cancel_url = "http://localhost:3000/cancel_page",
            //    return_url = "http://localhost:3000/success_page"
            //};

            //var itemList = new ItemList()
            //{
            //    items = new List<Item>()
            //  {
            //    new Item()
            //    {
            //      name = "Item Name",
            //      currency = "USD",
            //      price = "15",
            //      quantity = "5",
            //      sku = "sku"
            //    }
            //  }
            //};

            //var details = new Details()
            //{
            //    tax = "15",
            //    shipping = "10",
            //    subtotal = "75"
            //};

            //var amount = new Amount()
            //{
            //    currency = "USD",
            //    total = "100.00", // Total must be equal to sum of shipping, tax and subtotal.
            //    details = details
            //};

            //var transactionList = new List<Transaction>();

            //transactionList.Add(new Transaction()
            //{
            //    description = "Transaction description.",
            //    invoice_number = "228228228228228228",
            //    amount = amount,
            //    item_list = itemList
            //});

            //var payment = new Payment()
            //{
            //    intent = "sale",
            //    payer = payer,
            //    redirect_urls = redirUrls,
            //    transactions = transactionList
            //};


            //var createdPayment = payment.Create(CaptureOrderSample.apiContext);

            //var links = createdPayment.links.GetEnumerator();
            //while (links.MoveNext())
            //{
            //    var link = links.Current;
            //    if (link.rel.ToLower().Trim().Equals("approval_url"))
            //    {

            //    }
            //}

            //return createdPayment.links.Where(x => x.rel == "approval_url").Select(x => x.href).FirstOrDefault();



            // Construct a request object and set desired parameters
            // Here, OrdersCreateRequest() creates a POST request to /v2/checkout/orders
            //var order = new OrderRequest()
            //{
            //    CheckoutPaymentIntent = "CAPTURE",
            //    PurchaseUnits = new List<PurchaseUnitRequest>()
            //    {
            //        new PurchaseUnitRequest()
            //        {
            //            AmountWithBreakdown = new AmountWithBreakdown()
            //            {
            //                CurrencyCode = "USD",
            //                Value = "100.00"
            //            }
            //        }
            //    },
            //    ApplicationContext = new ApplicationContext()
            //    {
            //        ReturnUrl = "http://localhost:3000/success_page",
            //        CancelUrl = "http://localhost:3000/cancel_page"
            //    }
            //};
            var orderRequest = new OrderRequest()
            {
                CheckoutPaymentIntent = "CAPTURE",
                ApplicationContext = new ApplicationContext
                {
                    ReturnUrl = "https://api20200526110933.azurewebsites.net/success_page",
                    CancelUrl = "https://api20200526110933.azurewebsites.net/cancel_page",
                    BrandName = "Nakkis INC",
                    LandingPage = "BILLING",
                    UserAction = "CONTINUE",
                    ShippingPreference = "SET_PROVIDED_ADDRESS"
                },
                PurchaseUnits = new List<PurchaseUnitRequest>
            {
                new PurchaseUnitRequest{
                ReferenceId =  "PUHF",
                Description = "Sporting Goods",
                CustomId = "CUST-HighFashions",
                SoftDescriptor = "HighFashions",
                AmountWithBreakdown = new AmountWithBreakdown
                {
                    CurrencyCode = "USD",
                    Value = (Math.Round(currenctCart.TotalPrice * 0.13, 2) + currenctCart.TotalPrice).ToString(CultureInfo.InvariantCulture),
                    AmountBreakdown = new AmountBreakdown
                    {
                    ItemTotal = new Money
                    {
                        CurrencyCode = "USD",
                        Value = Math.Round(currenctCart.TotalPrice, 2).ToString(CultureInfo.InvariantCulture)
                    },
                    Shipping = new Money
                    {
                        CurrencyCode = "USD",
                        Value = "00.00"
                    },
                    //Handling = new Money
                    //{
                    //    CurrencyCode = "USD",
                    //    Value = "10.00"
                    //},
                    TaxTotal = new Money
                    {
                        CurrencyCode = "USD",
                        Value = Math.Round(currenctCart.TotalPrice * 0.13, 2).ToString(CultureInfo.InvariantCulture)
                    },
                    //ShippingDiscount = new Money
                    //{
                    //    CurrencyCode = "USD",
                    //    Value = "10.00"
                    //}
                    }
                },
                Items = items,
                ShippingDetail = new ShippingDetail
                {
                    Name = new Name
                    {
                    FullName = "John Doe"
                    },
                    AddressPortable = new AddressPortable
                    {
                    AddressLine1 = "123 Townsend St",
                    AddressLine2 = "Floor 6",
                    AdminArea2 = "San Francisco",
                    AdminArea1 = "CA",
                    PostalCode = "94107",
                    CountryCode = "US"
                    }
                }
                }
            }
            };


            // Call API with your client and get a response for your call
            var request = new OrdersCreateRequest();
            request.Prefer("return=representation");
            request.RequestBody(orderRequest);
            response = await CaptureOrderSample.client().Execute<OrdersCreateRequest>(request);
            var statusCode = response.StatusCode;
            Order result = response.Result<Order>();
            Console.WriteLine("Status: {0}", result.Status);
            Console.WriteLine("Order Id: {0}", result.Id);
            Console.WriteLine("Intent: {0}", result.CheckoutPaymentIntent);
            Console.WriteLine("Links:");
            foreach (LinkDescription link in result.Links)
            {
                Console.WriteLine("\t{0}: {1}\tCall Type: {2}", link.Rel, link.Href, link.Method);
            }
            return new { approval_url = result.Links.Where(x => x.Rel == "approve").Select(x => x.Href).FirstOrDefault(), orderId = result.Id };
        }
    }
}