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
        [HttpGet("{orderId:int}/items")]
        [ProducesResponseType(typeof(ResponsePagingDto<OrderReadDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseErrorDto), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllOrderItem([FromRoute] int orderId, [FromRoute] int itemId, CancellationToken cancellationToken)
        {
            var order = await service.GetOrderItemByIdAsync(orderId, cancellationToken);
            return StatusCode(StatusCodes.Status200OK, order);
        }


        [HttpDelete("{orderId:int}/items/{itemId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseErrorDto), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RemoveOrderItem([FromRoute] int orderId, [FromRoute] int itemId, CancellationToken cancellationToken)
        {
            await service.RemoveItemAsync(orderId, itemId, cancellationToken);
            return StatusCode(StatusCodes.Status204NoContent);
        }


        [HttpPatch("{orderId:int}/items/{itemId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseErrorDto), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateOrderItem([FromRoute] int orderId, [FromRoute] int itemId, [FromBody] OrderItemUpdateDto itemDto, CancellationToken cancellationToken)
        {
            await service.UpdateItemAsync(orderId, itemId, itemDto, cancellationToken);
            return StatusCode(StatusCodes.Status204NoContent);
        }


        [HttpPatch("{orderId:int}/status")]
        [ProducesResponseType(typeof(OrderReadDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseErrorDto), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateStatusOrder([FromRoute] int orderId, [FromBody] OrderStatusDto status, CancellationToken cancellationToken)
        {
            var order = await service.UpdateStatueOrderAsync(orderId, status, cancellationToken);
            return StatusCode(StatusCodes.Status200OK, order);
        }


        [HttpPost("{orderId:int}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseErrorDto), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddOrderItem([FromRoute] int orderId, [FromBody] OrderItemDto orderDto, CancellationToken cancellationToken)
        {
            await service.AddItemAsync(orderId, orderDto, cancellationToken);
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

            var order = await service.GetOrderByIdAsync(orderId, cancellationToken);
            return StatusCode(StatusCodes.Status200OK, order);
        }


        [HttpPost]
        [ProducesResponseType(typeof(OrderReadDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseErrorDto), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SaveOrder([FromBody] OrderDto orderDto, CancellationToken cancellationToken)
        {
            var order = await service.CreateOrderAsync(orderDto, cancellationToken);
            return StatusCode(StatusCodes.Status201Created, order);
        }


        [HttpPut("{orderId:int}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseErrorDto), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateOrder(int orderId, OrderUpdateDto orderDto, CancellationToken cancellationToken)
        {
            var order = await service.UpdateOrderAsync(orderId, orderDto, cancellationToken);
            return StatusCode(StatusCodes.Status201Created, order);
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