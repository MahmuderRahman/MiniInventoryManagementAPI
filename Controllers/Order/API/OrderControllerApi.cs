using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniInventoryManagementAPI.DTOs;
using MiniInventoryManagementAPI.Responses;
using MiniInventoryManagementAPI.Service;
using System.Net;

namespace MiniInventoryManagementAPI.Controllers.Order.API
{
    [ApiController]
    public class OrderControllerApi : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderControllerApi(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        [Route("api/order/create")]
        public IActionResult CreateOrder([FromBody] OrderDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                return Ok(new ApiResponse(_orderService.CreateOrder(dto)));
            }
            catch (DbUpdateException e)
            {
                var sqlError = ExceptionHelper.ThrowSqlException(e);
                return StatusCode((int)HttpStatusCode.BadRequest, sqlError);
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }


        [HttpGet]
        [Route("api/order/get-all-order")]
        public IActionResult GetAllOrders()
        {
            try
            {
                return Ok(new ApiResponse(_orderService.GetOrderList()));
            }
            catch (DbUpdateException e)
            {
                var sqlError = ExceptionHelper.ThrowSqlException(e);
                return StatusCode((int)HttpStatusCode.BadRequest, sqlError);
            }
            catch (Exception e)
            {
                var exceptionResponse = new ExceptionHelper(e);
                return StatusCode((int)HttpStatusCode.BadRequest, exceptionResponse);
            }
        }

    }
}
