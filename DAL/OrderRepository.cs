using DAL.Interfaces;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL
{
    public class OrderRepository : IOrderRepository
    {
        public bool Create(Order order)
        {
            using (var context = new NorthwindEntities())
            {
                context.Orders.Add(order);
                context.SaveChanges();
                return true;
            }
        }


        public Order Get(int id)
        {
            using (var context = new NorthwindEntities())
            {
                var result = context.Orders.FirstOrDefault(order => order.OrderID == id);
                return result;
            }
        }

        public IEnumerable<Order> GetAll()
        {
            using (var context = new NorthwindEntities())
            {
                var result = context.Orders.ToList();
                return result;
            }
        }

        public bool ChangeOrderDate(DateTime newOrderDate, int orderID)
        {
            using (var context = new NorthwindEntities())
            {
                var order = context.Orders.FirstOrDefault(o => o.OrderID == orderID);
                order.OrderDate = newOrderDate;
                context.SaveChanges();
                return true;
            }
        }

        public bool ChangeShippedDate(DateTime newShippedDate, int orderID)
        {
            using (var context = new NorthwindEntities())
            {
                var order = context.Orders.FirstOrDefault(o => o.OrderID == orderID);
                order.ShippedDate = newShippedDate;
                context.SaveChanges();
                return true;
            }
        }

        public bool DeleteOrder(Order order)
        {
            using (var context = new NorthwindEntities())
            {
                context.Orders.Remove(order);
                context.SaveChanges();
                return true;
            }
        }
    }
}
