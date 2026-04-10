using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using smakchet.application.DTOs;
using smakchet.application.DTOs.Error;
using smakchet.application.DTOs.Order;
using smakchet.application.DTOs.Success;
using smakchet.application.Interfaces.IOrder;

namespace smakchet.api.Controllers.V2026_01_01
{
    [ApiVersion("2026-01-01")]
    [Route("orders")]
    [Produces("application/json")]
    [ApiController]
    public class OrderController(IOrderService service) : ControllerBase
    {
        [HttpGet("{orderId:int}/items")]
        [ProducesResponseType(typeof(ResponseDto<OrderReadDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseDto<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllOrderItem(
            [FromRoute] int orderId,
            CancellationToken cancellationToken)
        {
            var items = await service.GetOrderItemByIdAsync(orderId, cancellationToken);
            return Ok(ResponseDto<OrderReadDto>.Ok(items));
        }

        [HttpGet("{orderId:int}/status")]
        [ProducesResponseType(typeof(ResponseDto<OrderReadDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseDto<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetStatueOrder([FromRoute] int orderId, CancellationToken cancellationToken)
        {
            var order = await service.GetStatusOrderAsync(orderId, cancellationToken);
            return Ok(ResponseDto<OrderReadDto>.Ok(order));
        }


        [HttpGet("{orderId:int}/item")]
        [ProducesResponseType(typeof(ResponseDto<OrderReadDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseDto<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetOrderItem(int orderId, CancellationToken cancellationToken)
        {

            var order = await service.GetOrderByIdAsync(orderId, cancellationToken);
            return Ok(ResponseDto<OrderReadDto>.Ok(order));
        }


        [HttpGet]
        [ProducesResponseType(typeof(ResponseDto<ResponsePagingDto<OrderReadDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseDto<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPag([FromQuery] PaginationQueryParams param)
        {
            var categories = await service.GetOrderPagedAsync(param);
            return Ok(ResponseDto<ResponsePagingDto<OrderReadDto>>.Ok(categories));
        }


        [HttpGet("{orderId:int}")]
        [ProducesResponseType(typeof(ResponseDto<OrderReadDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseDto<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetOrder(int orderId, CancellationToken cancellationToken)
        {

            var order = await service.GetOrderByIdAsync(orderId, cancellationToken);
            return Ok(ResponseDto<OrderReadDto>.Ok(order));
        }


        [HttpPost]
        [ProducesResponseType(typeof(ResponseDto<OrderReadDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseDto<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SaveOrder([FromBody] OrderDto orderDto, CancellationToken cancellationToken)
        {
            var order = await service.CreateOrderAsync(orderDto, cancellationToken);
            return StatusCode(StatusCodes.Status201Created, ResponseDto<OrderReadDto>.Ok(order, "Order created successfully"));
        }


        [HttpPut("{orderId:int}")]
        [ProducesResponseType(typeof(ResponseDto<OrderReadDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseDto<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateOrder([FromRoute] int orderId, [FromBody] OrderUpdateDto payload, CancellationToken cancellationToken)
        {
            var order = await service.UpdateOrderAsync(orderId, payload, cancellationToken);
            return Ok(ResponseDto<OrderReadDto>.Ok(order, "Order updated successfully"));
        }


        [HttpDelete("{orderId:int}")]
        [ProducesResponseType(typeof(ResponseDto<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseDto<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteOrder(int orderId, CancellationToken cancellationToken)
        {
            await service.DeleteOrderAsync(orderId, cancellationToken);
            return Ok(ResponseDto<object>.Ok(null, "Order deleted successfully"));
        }
    }
}