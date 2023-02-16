using EShop.Domain.DomainModels;
using EShop.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Service.Interface
{
    public interface IShoppingCartService
    {
        ShoppingCartDto GetShoppingCartInfo(string userId);
        bool DeleteProductFromShoppingCart(string userId, int productId);
        bool OrderNow(string userId);
    }
}
