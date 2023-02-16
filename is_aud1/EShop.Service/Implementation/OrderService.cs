using EShop.Domain.DomainModels;
using EShop.Repository.Interface;
using EShop.Service.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Repository.Implementation
{
    public class OrderService : IOrderService
    {
        public readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        public List<Order> GetAllOrders()
        {
            return _orderRepository.GetAllOrders();
        }

        public Order GetOrderDetails(BaseEntity model)
        {
            return _orderRepository.GetOrderDetails(model);
        }
    }
}
