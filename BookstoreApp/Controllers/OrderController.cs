using Manager.Interface;
using Microsoft.AspNetCore.Mvc;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookstoreApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : Controller
    {
        private readonly IOrderManager manager;

        public OrderController(IOrderManager manager)
        {
            this.manager = manager;

        }

        [HttpPost]
        [Route("PlaceOrders")]
        public IActionResult AddOrder([FromBody] List<CartModel> orderDetails)
        {
            try
            {
                var result = this.manager.AddOrder(orderDetails);
                if (result)
                {

                    return this.Ok(new ResponseModel<string>() { Status = true, Message = "Order placed successfully" });
                }
                else
                {

                    return this.BadRequest(new ResponseModel<string>() { Status = false, Message = "Failed to place order, Try again" });
                }
            }
            catch (Exception ex)
            {

                return this.NotFound(new ResponseModel<string>() { Status = false, Message = ex.Message });

            }
        }
    }
}
