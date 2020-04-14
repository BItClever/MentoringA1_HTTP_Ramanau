using DAL.Models;
using System;
using System.Collections.Generic;

namespace DAL.Interfaces
{
    public interface IOrderRepository
    {
        IEnumerable<Order> GetAll();
        Order Get(int id);
        bool Create(Order order);
        bool ChangeOrderDate(DateTime newOrderDate, int orderID);
        bool ChangeShippedDate(DateTime newShippedDate, int orderID);
        bool DeleteOrder(Order order);
    }
}
