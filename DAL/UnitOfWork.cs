using DAL.Interfaces;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL
{
    public class UnitOfWork : IDisposable
    {
        private readonly IOrderRepository orderRepository;
        private bool disposed = false;

        public UnitOfWork(IOrderRepository orderRepository)
        {
            this.orderRepository = orderRepository;
        }

        public bool AddOrder(Order order)
        {
            return orderRepository.Create(order);
        }

        public bool ChangeOrderDate(DateTime orderDate, int orderId)
        {
            return orderRepository.ChangeOrderDate(orderDate, orderId);
        }

        public bool ChangeShippedDate(DateTime shippedDate, int orderId)
        {
            return orderRepository.ChangeShippedDate(shippedDate, orderId);
        }

        public Order GetOrderById(int orderId)
        {
            return orderRepository.Get(orderId);
        }

        public bool DeleteOrder(int orderId)
        {
            Order orderToDelete = orderRepository.Get(orderId);
            if (orderToDelete == null)
            {
                return false;
            }
            else
            {
                return orderRepository.DeleteOrder(orderToDelete);
            }
        }

        public List<Order> GetAllOrders()
        {
            return orderRepository.GetAll().ToList();
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {

                }
                disposed = true;
            }
        }
        ~UnitOfWork()
        {
            Dispose(false);
        }
    }
}
