using EShop.Domain.DomainModels;
using EShop.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Service.Interface
{
    public interface IProductService
    {
        List<Product> GetAllProducts();
        Product GetDetailsForProduct(int id);
        void CreateNewProduct(Product p);
        void UpdateExistingProduct(Product p);
        ShoppingCartDto GetShoppingCartInfo(int id);
        void DeleteProduct(int id);
        bool AddToShoppingCart(AddToShoppingCartDto item, string userId);

    }
}
