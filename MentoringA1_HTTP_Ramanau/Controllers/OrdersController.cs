using DAL;
using DAL.Models;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentoringA1_HTTP_Ramanau.Controllers
{
    [ApiController]
    [Route("orders")]
    public class OrdersController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IDistributedCache _cache;
        public OrdersController(UnitOfWork unitOfWork, IConfiguration config, IDistributedCache cache)
        {
            _unitOfWork = unitOfWork;
            _cache = cache;
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            else
            {
                var result = _unitOfWork.GetOrderById(id.Value);
                if (result == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(result);
                }
            }
        }

        [HttpGet]
        [EnableQuery()]
        public async Task<IActionResult> Get()
        {
            var cacheKey = "Orders";
            List<Order> orders;
            string serializedOrders;

            var encodedOrders = await _cache.GetAsync(cacheKey);

            if (encodedOrders != null)
            {
                serializedOrders = Encoding.UTF8.GetString(encodedOrders);
                orders = JsonConvert.DeserializeObject<List<Order>>(serializedOrders);
            }
            else
            {
                orders = _unitOfWork.GetAllOrders().OrderBy(o => o.OrderID).ToList();
                serializedOrders = JsonConvert.SerializeObject(orders);
                encodedOrders = Encoding.UTF8.GetBytes(serializedOrders);
                var options = new DistributedCacheEntryOptions()
                                .SetSlidingExpiration(TimeSpan.FromMinutes(5))
                                .SetAbsoluteExpiration(DateTime.Now.AddHours(6));
                await _cache.SetAsync(cacheKey, encodedOrders, options);
            }
            return Ok(orders);
        }


        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var result = _unitOfWork.DeleteOrder(id);
            if (result)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public IActionResult Post(Order order)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.AddOrder(order);
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }


        [HttpPut]
        public IActionResult ChangeOrderDate(DateTime newOrderDate, int orderId)
        {
            var result = _unitOfWork.ChangeOrderDate(newOrderDate, orderId);
            if (result)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPut]
        public IActionResult ChangeShippedDate(DateTime newShippedDate, int orderId)
        {
            var result = _unitOfWork.ChangeShippedDate(newShippedDate, orderId);
            if (result)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
