using DAL.Interfaces;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ILogger _logger;

        public OrderRepository(ILogger logger)
        {
            _logger = logger;
        }

        public bool Create(Order order)
        {
            _logger.LogInfo("Adding order to database");
            try
            {
                using (var context = new NorthwindEntities())
                {

                    context.Orders.Add(order);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e);
                throw;
            }
        }


        public Order Get(int id)
        {
            _logger.LogInfo($"Gettting order by Id ={id} from database");
            try
            {
                using (var context = new NorthwindEntities())
                {
                    var result = context.Orders.FirstOrDefault(order => order.OrderID == id);
                    return result;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e);
                throw;
            }
        }

        public IEnumerable<Order> GetAll()
        {
            _logger.LogInfo($"Gettting all orders from database");
            try
            {
                using (var context = new NorthwindEntities())
                {
                    var result = context.Orders.ToList();
                    return result;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e);
                throw;
            }
        }

        public bool ChangeOrderDate(DateTime newOrderDate, int orderID)
        {
            _logger.LogInfo($"Changing newOrderDate by Id ={orderID} from database");
            try
            {
                using (var context = new NorthwindEntities())
                {
                    var order = context.Orders.FirstOrDefault(o => o.OrderID == orderID);
                    order.OrderDate = newOrderDate;
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e);
                throw;
            }
        }

        public bool ChangeShippedDate(DateTime newShippedDate, int orderID)
        {
            _logger.LogInfo($"Changing newShippedDate by Id ={orderID} from database");
            try
            {
                using (var context = new NorthwindEntities())
                {
                    var order = context.Orders.FirstOrDefault(o => o.OrderID == orderID);
                    order.ShippedDate = newShippedDate;
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e);
                throw;
            }
        }

        public bool DeleteOrder(Order order)
        {
            _logger.LogInfo($"Deleting order by Id ={order.OrderID} from database");
            try
            {
                using (var context = new NorthwindEntities())
                {
                    context.Orders.Remove(order);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e);
                throw;
            }
        }
    }
}
