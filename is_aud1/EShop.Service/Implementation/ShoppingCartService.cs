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
    public class ShoppingCartService : IShoppingCartService
    {
        public readonly IRepository<ProductsInOrder> _productsInOrderRepository;
        public readonly IRepository<ShoppingCart> _shoppingCartRepository;
        public readonly IUserRepository _userRepository;
        public readonly IRepository<Order> _orderRepository;
        private readonly IRepository<EmailMessage> _emailMessageRepository;


        public ShoppingCartService(IRepository<ProductsInOrder> productsInOrderRepository, IRepository<ShoppingCart> shoppingCartRepository, IUserRepository userRepository, IRepository<Order> orderRepository, IRepository<EmailMessage> emailMessageRepository)
        {
            _productsInOrderRepository = productsInOrderRepository;
            _shoppingCartRepository = shoppingCartRepository;
            _userRepository = userRepository;
            _orderRepository = orderRepository;
            _emailMessageRepository = emailMessageRepository;
        }

        public bool DeleteProductFromShoppingCart(string userId, int productId)
        {
            if (userId != null && productId != null)
            {
                var loggedUser = _userRepository.Get(userId);
                var userShoppingCart = loggedUser.UserShoppingCart;
                var itemToDelete = userShoppingCart.ProductsInShoppingCarts.Where(z => z.ProductId == productId).FirstOrDefault();

                userShoppingCart.ProductsInShoppingCarts.Remove(itemToDelete);

                _shoppingCartRepository.Update(userShoppingCart);

                return true;
            }
            return false;
        }

        public ShoppingCartDto GetShoppingCartInfo(string userId)
        {
            var user = _userRepository.Get(userId);

            var userShoppingCart = user.UserShoppingCart;

            var productList = userShoppingCart.ProductsInShoppingCarts.Select(z => new
            {
                Quantity = z.Quantity,
                ProductPrice = z.Product.ProductPrice,
                ProductSize = z.ProductSize
            });

            int totalPrice = 0;

            foreach (var item in productList)
            {
                totalPrice += item.ProductPrice * item.Quantity;
            }

            // some kind of constructor - "set method option"
            ShoppingCartDto model = new ShoppingCartDto
            {
                ProductsInShoppingCarts = userShoppingCart.ProductsInShoppingCarts.ToList(),
                TotalPrice = totalPrice
            };

            return model;
        }

        public bool OrderNow(string userId)
        {
            var user = _userRepository.Get(userId);

            var userShoppingCart = user.UserShoppingCart;

            EmailMessage mail = new EmailMessage();
            mail.MailTo = user.Email;
            mail.Subject = "Sucessfuly created order!";
            mail.Status = false; // na pochetok e false

            Order newOrder = new Order
            {
                UserId = user.Id,
                OrderedBy = user
            };

            _orderRepository.Insert(newOrder);

            //orderId has not already been saved to the database. It will be transfered as soon as it saves. No need to save to database this soon
            List<ProductsInOrder> productsInOrders = userShoppingCart.ProductsInShoppingCarts.Select(z => new ProductsInOrder
            {
                Product = z.Product,
                ProductId = z.ProductId,
                Order = newOrder,
                OrderId = newOrder.Id,
                Quantity = z.Quantity
            }).ToList();

            // email message start

            StringBuilder sb = new StringBuilder();

            var totalPrice = 0.0;

            sb.AppendLine("Your order is completed. The order conatins: ");

            for (int i = 1; i <= productsInOrders.Count(); i++)
            {
                var currentItem = productsInOrders[i - 1];
                totalPrice += currentItem.Quantity * currentItem.Product.ProductPrice;
                sb.AppendLine(i.ToString() + ". " + currentItem.Product.ProductName + " with quantity of: " + currentItem.Quantity + " and price of: $" + currentItem.Product.ProductPrice);
            }

            sb.AppendLine("Total price for your order: " + totalPrice.ToString());

            mail.Content = sb.ToString();

            // email message end

            foreach (var item in productsInOrders)
            {
                _productsInOrderRepository.Insert(item);
            }

            user.UserShoppingCart.ProductsInShoppingCarts.Clear();
            _userRepository.Update(user);
            _emailMessageRepository.Insert(mail);

            return true;
        }
    }
}
