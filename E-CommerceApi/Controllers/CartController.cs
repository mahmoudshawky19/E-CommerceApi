using E_CommerceApi.DTOs;
using E_CommerceApi.Models;
using E_CommerceApi.Repositery;
using E_CommerceApi.Repositery.InterfaceCategory;
 using Microsoft.AspNetCore.Authorization;
 using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;

namespace E_CommerceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartRepository cartRepositery;
         private readonly UserManager<ApplicationUser> userManager;
        public CartController(ICartRepository cartRepositery, UserManager<ApplicationUser> userManager)
        { 
            this.cartRepositery = cartRepositery;
            this.userManager = userManager;
        }
        [HttpPost("AddToCart")]
        public IActionResult AddToCart(int count, int productId)
        {
            var user = userManager.GetUserId(User);
            Cart cart = new Cart()
            {
                Count = count,
                ProductId = productId,
                ApplicationUserId = user
            };
            var cartProduct = cartRepositery.GetOne(expression: e => e.ProductId == productId && e.ApplicationUserId == user);

            if (cartProduct == null)
            {
                cartRepositery.Add(cart);
            }
            else
            {
                cartProduct.Count += count;
            }
            cartRepositery.Commit();

            return Ok();
        }
        [HttpGet("Index")]
        public IActionResult Index()
        {

            var ApplicationUserId = userManager.GetUserId(User);

            var cartProduct = cartRepositery.GetAll([e => e.Product], e => e.ApplicationUserId == ApplicationUserId).ToList();
            ShoppingCart shoppingCart = new()
            {
                carts = cartProduct,
                totalCarts = (double)cartProduct.Sum(e => e.Product.Price * e.Count)
            };
 
            return Ok(shoppingCart);
        }
        [HttpPut("Increment")]

        public IActionResult Increment(int productId)
        {
            var ApplicationUserId = userManager.GetUserId(User);

            var product = cartRepositery.GetOne(expression: e => e.ApplicationUserId == ApplicationUserId && e.ProductId == productId);

            if (product != null)
            {
                product.Count++;
                cartRepositery.Commit();

                return Ok();
            }

            return NotFound();
        }
        [HttpPut("Decrement")]

        public IActionResult Decrement(int productId)
        {
            var ApplicationUserId = userManager.GetUserId(User);

            var product = cartRepositery.GetOne(expression: e => e.ApplicationUserId == ApplicationUserId && e.ProductId == productId);

            if (product != null)
            {
                product.Count--;
                if (product.Count > 0)

                    cartRepositery.Commit();

                else
                    product.Count = 1;

                return  Ok();
            }

            return NotFound();
        }
        [HttpPut("Delete")]
        public IActionResult Delete(int productId)
        {
            var ApplicationUserId = userManager.GetUserId(User);

            var product = cartRepositery.GetOne(expression: e => e.ApplicationUserId == ApplicationUserId && e.ProductId == productId);

            if (product != null)
            {
                cartRepositery.Delete(product);
                cartRepositery.Commit();

                return Ok();
            }

            return NotFound();
        }
        [HttpPost("Pay")]
        public IActionResult Pay()
        {
            var ApplicationUserId = userManager.GetUserId(User);

            var cartProduct = cartRepositery.GetAll([e => e.Product], e => e.ApplicationUserId == ApplicationUserId).ToList();

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                SuccessUrl = $"{Request.Scheme}://{Request.Host}/checkout/success",
                CancelUrl = $"{Request.Scheme}://{Request.Host}/checkout/cancel",
            };

            foreach (var item in cartProduct)
            {
                options.LineItems.Add(new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "egp",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Product.Name,
                        },
                        UnitAmount = (long)item.Product.Price * 100,
                    },
                    Quantity = item.Count,
                });
            }

            var service = new SessionService();
            var session = service.Create(options);
            return Created(session.Url , cartProduct);
        }

    }
}
