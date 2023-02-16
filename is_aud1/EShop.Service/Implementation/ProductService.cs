using EShop.Domain.DomainModels;
using EShop.Domain.DTO;
using EShop.Domain.Relationships;
using EShop.Repository.Interface;
using EShop.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EShop.Service.Implementation
{
    public class ProductService : IProductService
    {
        public readonly IRepository<Product> _productRepository;
        public readonly IUserRepository _userRepository;
        public readonly IRepository<ProductsInShoppingCart> _productsInShoppingCartRepository;

        public ProductService(IRepository<Product> productRepository, IUserRepository userRepository, IRepository<ProductsInShoppingCart> productsInShoppingCartRepository)
        {
            _productRepository = productRepository;
            _userRepository = userRepository;
            _productsInShoppingCartRepository = productsInShoppingCartRepository;
        }
        public bool AddToShoppingCart(AddToShoppingCartDto item, string userId)
        {
            var user = _userRepository.Get(userId);

            var userShoppingCart = user.UserShoppingCart;

            if (userShoppingCart != null)
            {
                var product = GetDetailsForProduct(item.ProductId);

                if (product != null)
                {
                    ProductsInShoppingCart itemToAdd = new ProductsInShoppingCart
                    {
                        Product = product,
                        ProductId = product.Id,
                        ShoppingCart = userShoppingCart,
                        ProductSize = item.ProductSize,
                        Quantity = item.Quantity
                    };
                    _productsInShoppingCartRepository.Insert(itemToAdd);
                    return true;
                }
                return false;
            }
            return false;
            
        }

        public void CreateNewProduct(Product p)
        {
            this._productRepository.Insert(p);
        }

        public void DeleteProduct(int id)
        {
            var product = _productRepository.Get(id);
            this._productRepository.Delete(product);
        }

        public List<Product> GetAllProducts()
        {
            return _productRepository.GetAll().ToList();
        }

        public Product GetDetailsForProduct(int id)
        {
            return _productRepository.Get(id);
        }

        public ShoppingCartDto GetShoppingCartInfo(int id)
        {
            throw new NotImplementedException();
        }

        public void UpdateExistingProduct(Product p)
        {
            _productRepository.Update(p);
        }
    }
}
