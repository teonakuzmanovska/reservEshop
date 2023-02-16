using EShop.Domain.DomainModels;
using EShop.Domain.DTO;
using EShop.Domain.Identity;
using EShop.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EShop.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminApiController : ControllerBase
    {
        public readonly IOrderService _orderService;
        public readonly UserManager<ShopApplicationUser> _userManager;

        public AdminApiController(IOrderService orderService, UserManager<ShopApplicationUser> userManager)
        {
            _orderService = orderService;
            _userManager = userManager;
        }
        [HttpGet("[action]")]
        public List<Order> GetAllActiveOrders()
        {
            return _orderService.GetAllOrders();
        }
        [HttpPost("[action]")]
        public Order GetOrderDetails(BaseEntity model)
        {
            return _orderService.GetOrderDetails(model);
        }
        [HttpPost("[action]")]
        public bool ImportAllUsers(List<UserRegistrationDto> model)
        {
            bool status = true;
            foreach (var item in model)
            {
                var userCheck = _userManager.FindByEmailAsync(item.Email).Result;
                if (userCheck == null)
                {
                    var newUser = new ShopApplicationUser
                    {
                        UserName = item.Email,
                        NormalizedUserName = item.Email,
                        Email = item.Email,
                        EmailConfirmed = true,
                        PhoneNumberConfirmed = true,
                        UserShoppingCart = new ShoppingCart()
                    };
                    var result = _userManager.CreateAsync(newUser, item.Password).Result;
                    status = status & result.Succeeded;
                }
                else
                {
                    continue;
                }
            }

            return status;
        }
    }
}
