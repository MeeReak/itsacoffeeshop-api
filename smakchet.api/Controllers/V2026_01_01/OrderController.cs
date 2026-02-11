using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using smakchet.application.DTOs;
using smakchet.application.DTOs.Error;
using smakchet.application.DTOs.Order;
using smakchet.application.DTOs.OrderItem;
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
        [HttpPost("{orderId:int}/add")]
        [ProducesResponseType(typeof(OrderItemDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseErrorDto), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddOrderItem([FromRoute] int orderId, [FromBody] OrderItemDto orderDto, CancellationToken cancellationToken)
        {
            await service.AddItemAsync(orderId, orderDto, cancellationToken);
            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpPost("{orderId:int}/remove/{itemId:int}")]
        [ProducesResponseType(typeof(OrderItemDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseErrorDto), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RemoveOrderItem([FromRoute] int orderId, [FromRoute] int itemId, CancellationToken cancellationToken)
        {
            await service.RemoveItemAsync(orderId, itemId, cancellationToken);
            return StatusCode(StatusCodes.Status201Created);
        }


        [HttpGet]
        [ProducesResponseType(typeof(ResponsePagingDto<OrderReadDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseErrorDto), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPag([FromQuery] PaginationQueryParams param)
        {
            var categories = await service.GetOrderPagedAsync(param);
            return StatusCode(StatusCodes.Status200OK, categories);
        }


        [HttpGet("{orderId:int}")]
        [ProducesResponseType(typeof(OrderReadDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseErrorDto), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetOrder(int orderId, CancellationToken cancellationToken)
        {

            var Order = await service.GetOrderByIdAsync(orderId, cancellationToken);
            return StatusCode(StatusCodes.Status200OK, Order);
        }


        [HttpPost]
        [ProducesResponseType(typeof(OrderDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseErrorDto), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SaveOrder([FromBody] OrderDto orderDto, CancellationToken cancellationToken)
        {
            var Order = await service.CreateOrderAsync(orderDto, cancellationToken);
            return StatusCode(StatusCodes.Status201Created, Order);
        }


        [HttpPut("{orderId:int}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseErrorDto), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateOrder(int orderId, OrderUpdateDto orderDto, CancellationToken cancellationToken)
        {
            var Order = await service.UpdateOrderAsync(orderId, orderDto, cancellationToken);
            return StatusCode(StatusCodes.Status201Created, Order);
        }


        [HttpDelete("{orderId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseErrorDto), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteOrder(int orderId, CancellationToken cancellationToken)
        {
            await service.DeleteOrderAsync(orderId, cancellationToken);
            return StatusCode(StatusCodes.Status204NoContent);
        }
    }
}