using EShop.Domain.DomainModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EShop.Domain.Identity
{
    // inherits from IdentityUser
    public class ShopApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Address { get; set; }

        // object of the shopping cart
        public virtual ShoppingCart UserShoppingCart { get; set; }
    }
}
