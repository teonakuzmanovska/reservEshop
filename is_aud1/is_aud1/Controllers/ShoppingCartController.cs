using EShop.Domain.DomainModels;
using EShop.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EShop.Web.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly IShoppingCartService _shoppingCartService;

        public ShoppingCartController(IShoppingCartService shoppingCartService)
        {
            _shoppingCartService = shoppingCartService;
        }
        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var model = _shoppingCartService.GetShoppingCartInfo(userId);

            return View(model);
        }

        // the parameter is called id and not ProductId because Startup.cs requires so.
        // you should either use id here, or change it in Startup.cs
        public IActionResult DeleteFromShoppingCart(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            _shoppingCartService.DeleteProductFromShoppingCart(userId, id);

            return RedirectToAction("Index");
        }

        public Boolean OrderNow()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = _shoppingCartService.OrderNow(userId);

            return result;

        }

        public IActionResult PayOrder(string stripeEmail, string stripeToken)
        {
            var customerService = new CustomerService();
            var chargeService = new ChargeService();
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var order = this._shoppingCartService.GetShoppingCartInfo(userId); //ovde kje padne !!

            var customer = customerService.Create(new CustomerCreateOptions
            {
                Email = stripeEmail,
                Source = stripeToken
            });

            var charge = chargeService.Create(new ChargeCreateOptions
            {
                Amount = (Convert.ToInt32(order.TotalPrice) * 100),
                Description = "EShop Application Payment",
                Currency = "usd",
                Customer = customer.Id
            });

            // ne dozvoluvame vnesuvanje u baza i chistenje na cart bez uplata

            if (charge.Status == "succeeded")
            {
                var result = this.OrderNow();

                if (result)
                {
                    return RedirectToAction("Index", "ShoppingCart");
                }
                else
                {
                    return RedirectToAction("Index", "ShoppingCart");
                }
            }

            return RedirectToAction("Index", "ShoppingCart");
        }
    }
}
