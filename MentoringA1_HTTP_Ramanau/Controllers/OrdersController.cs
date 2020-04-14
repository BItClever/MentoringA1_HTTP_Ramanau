using DAL;
using DAL.Models;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace MentoringA1_HTTP_Ramanau.Controllers
{
    [ApiController]
    [Route("orders")]
    public class OrdersController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;
        public OrdersController(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
        public IActionResult Get()
        {
            var result = _unitOfWork.GetAllOrders().OrderBy(o => o.OrderID);
            return Ok(result);
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
