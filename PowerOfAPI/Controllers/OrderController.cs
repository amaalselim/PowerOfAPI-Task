using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PowerOf.Application.DTO_s;
using PowerOf.Core.Entities;
using PowerOf.Core.IUnitOfWork;
using System.Security.Claims;

namespace PowerOfAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        [HttpGet("GetAllOrders")]
        public async Task<ActionResult<OrderDTO>> GetAllOrders()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var Order = await _unitOfWork._orderRepository.GetOrderAsync(userId);
            if (Order == null)
            {
                return Ok(new { Message = "No items available in the Order." });
            }
            return Ok(Order);
        }

        [HttpPost("CreateOrder")]
        public async Task<IActionResult> CreateOrder([FromBody] OrderItemDTO OrderItemDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _unitOfWork._orderRepository.AddToOrderAsync(userId, OrderItemDTO);
            return CreatedAtAction(nameof(GetAllOrders), new { userId=userId }, OrderItemDTO);
        }

        [HttpPut("CancelOrder/{orderId}")]
        public async Task<IActionResult> CancelOrder(int orderId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User is not authenticated.");
            }
            await _unitOfWork._orderRepository.CancelOrderAsync(orderId);
            return Ok("Order has been cancelled.");
        }

        [HttpPut("approveOrder/{orderId}")]
        public async Task<IActionResult> ApproveOrder(int orderId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User is not authenticated.");
            }

            var success = await _unitOfWork._orderRepository.ApproveOrderAsync(orderId);
            if (!success)
            {
                return NotFound("Order cannot be approved.");
            }

            return Ok("Order has been approved.");
        }



    }
}
