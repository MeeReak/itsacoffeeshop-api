using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using smakchet.application.Constants;
using smakchet.application.Constants.Enum;
using smakchet.application.DTOs;
using smakchet.application.DTOs.Order;
using smakchet.application.DTOs.OrderItem;
using smakchet.application.DTOs.Success;
using smakchet.application.Exceptions;
using smakchet.application.Helpers;
using smakchet.application.Interfaces;
using smakchet.application.Interfaces.IOrder;
using smakchet.application.Interfaces.IProduct;
using smakchet.dal.Models;

namespace smakchet.application.Services
{
    public class OrderService(
        IOrderRepository orderRepository,
        IProductRepository productRepository,
        IMapper<Order, OrderReadDto, OrderDto, OrderUpdateDto> orderMapper,
        IMapper<OrderItem, OrderItemReadDto, OrderItemDto, OrderItemUpdateDto> orderItemMapper,
        ILogger<OrderService> logger,
        IHttpContextAccessor contextAccessor) : IOrderService
    {
        public async Task<OrderReadDto> CreateOrderAsync(OrderDto orderDto, CancellationToken cancellationToken)
        {
            var existing = await orderRepository.Query()
                .Where(o => o.Number == orderDto.Number
                            && o.CreatedAt!.Date == DateTime.Today)
                .FirstOrDefaultAsync(cancellationToken);
            if (existing != null)
            {
                logger.LogError(ErrorMessageConstants.AlreadyExists, "Order", existing.Number);
                throw new DuplicateException(
                    string.Format(ErrorMessageConstants.AlreadyExists, "Order", existing.Number),
                    ErrorCodeConstants.Conflict);
            }

            try
            {
                var mapped = orderMapper.ToEntity(orderDto);
                await orderRepository.AddAsync(mapped, cancellationToken);
                await orderRepository.SaveChangesAsync(cancellationToken);
                logger.LogInformation(SuccessMessageConstants.Created, "Order");
                return orderMapper.ToReadDto(mapped);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ErrorMessageConstants.OperationFailed, "CreateOrder");
                throw;
            }
        }



        public async Task DeleteOrderAsync(int orderId, CancellationToken cancellationToken)
        {
            var existing = await orderRepository.GetByIdAsync(orderId, cancellationToken);
            if (existing == null)
            {
                logger.LogError(ErrorMessageConstants.ResourceNotFoundById, "Order", orderId);
                throw new NotFoundException(string.Format(ErrorMessageConstants.ResourceNotFoundById, "Order", orderId),
                    ErrorCodeConstants.NotFound);
            }
          
            try
            {
                await orderRepository.DeleteAsync(existing, cancellationToken);
                logger.LogInformation(SuccessMessageConstants.Deleted, "Order");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ErrorMessageConstants.OperationFailed, "DeleteOrder");
                throw;  
            }
        }



        public async Task<OrderReadDto?> GetOrderByIdAsync(int orderId, CancellationToken cancellationToken)
        {
            var existing = await orderRepository.GetByIdAsync(orderId, cancellationToken);
            if (existing == null)
            {
                logger.LogError(ErrorMessageConstants.ResourceNotFoundById, "Order", orderId);
                throw new NotFoundException(string.Format(ErrorMessageConstants.ResourceNotFoundById, "Order", orderId),
                    ErrorCodeConstants.NotFound);
            }
            var result = orderMapper.ToReadDto(existing);
            logger.LogInformation(SuccessMessageConstants.Retrieved, "Order");
            return result;
        }



        public async Task<ResponsePagingDto<OrderReadDto>> GetOrderPagedAsync(PaginationQueryParams param)
        {
            IQueryable<Order> query = orderRepository.Query()
                .AsNoTracking()
                .Include(o => o.OrderItems);

            if (!string.IsNullOrWhiteSpace(param.Search))
            {
                if (int.TryParse(param.Search, out int number))
                {
                    query = query.Where(o => o.Number == number);
                }
            }

            var orders = await query.ToPagedResultAsync(
                param.Skip,
                param.Top,
                orderMapper.ToReadDto,
                contextAccessor.HttpContext);

            logger.LogInformation(SuccessMessageConstants.Retrieved, "Order");

            return orders;
        }



        public async Task<OrderReadDto?> UpdateOrderAsync(int orderId, OrderUpdateDto orderDto, CancellationToken cancellationToken)
        {
            var existing = await orderRepository.GetByIdAsync(orderId, cancellationToken);
            if (existing == null)
            {
                logger.LogError(ErrorMessageConstants.ResourceNotFoundById, "Order", orderId);
                throw new NotFoundException(string.Format(ErrorMessageConstants.ResourceNotFoundById, "Order", orderId),
                    ErrorCodeConstants.NotFound);
            }

            try
            {
                orderMapper.UpdateEntity(existing, orderDto);
                await orderRepository.UpdateAsync(existing, cancellationToken);
                logger.LogInformation(SuccessMessageConstants.Updated, "Order");
                return orderMapper.ToReadDto(existing);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ErrorMessageConstants.OperationFailed, "UpdateOrder");
                throw;
            }
        }



        public void Recalculate(Order order)
        {
            decimal subtotal = 0;

            foreach (var item in order.OrderItems)
            {
                decimal unitPrice = item.Size switch
                {
                    (int)OrderItemSizeEnum.Small => item.Price,
                    (int)OrderItemSizeEnum.Medium => item.Price + 1m,
                    (int)OrderItemSizeEnum.Large => item.Price + 2m,
                    _ => item.Price
                };

                subtotal += unitPrice * item.Quantity;
            }

            decimal taxRate = 0;
            decimal tax = subtotal * taxRate;

            order.Subtotal = subtotal;
            order.Total = subtotal + tax;
            order.UpdatedAt = DateTime.UtcNow;
        }




        public async Task AddItemAsync(int orderId, OrderItemDto itemDto, CancellationToken ct)
        {
            var product = await productRepository.GetByIdAsync(itemDto.ProductId, ct);
            if (product == null)
            {
                logger.LogError(ErrorMessageConstants.ResourceNotFoundById, "Product", itemDto.ProductId);
                throw new NotFoundException(
                    string.Format(ErrorMessageConstants.ResourceNotFoundById, "Product", itemDto.ProductId),
                    ErrorCodeConstants.NotFound);
            }

            var order = await orderRepository.GetOrderWithItemsAsync(orderId, ct);
            if (order == null)
            {
                logger.LogError(ErrorMessageConstants.ResourceNotFoundById, "Order", orderId);
                throw new NotFoundException(
                    string.Format(ErrorMessageConstants.ResourceNotFoundById, "Order", orderId),
                    ErrorCodeConstants.NotFound);
            }

            if (order.Status != (int)OrderStatusEnum.Pending)
                throw new BadRequestException("Order is not open");

            var existingItem = order.OrderItems
                .FirstOrDefault(i => i.ProductId == itemDto.ProductId && i.Size == (int)itemDto.Size);

            if (existingItem != null)
            {
                var newQty = existingItem.Quantity + itemDto.Quantity;
                if (newQty > 10)  
                    throw new BadRequestException("Max quantity per item is 10");

                existingItem.Quantity = newQty;
                existingItem.Note = itemDto.Note;  
            }
            else
            {
                var mapped = orderItemMapper.ToEntity(itemDto);
                order.OrderItems.Add(mapped);
            }

            Recalculate(order);

            await orderRepository.SaveChangesAsync(ct);

            logger.LogInformation("Item added to order {OrderId}", orderId);
        }




        public async Task RemoveItemAsync(int orderId, int itemId, CancellationToken ct)
        {
            var order = await orderRepository.GetOrderWithItemsAsync(orderId, ct);
            if (order == null)
                throw new NotFoundException(
                    string.Format(ErrorMessageConstants.ResourceNotFoundById, "Order", orderId),
                    ErrorCodeConstants.NotFound);

            if (order.Status != (int)OrderStatusEnum.Pending)
                throw new BadRequestException("Order is not open");

            var item = order.OrderItems.FirstOrDefault(i => i.Id == itemId);
            if (item == null)
                throw new NotFoundException(
                    string.Format(ErrorMessageConstants.ResourceNotFoundById, "Item", itemId),
                    ErrorCodeConstants.NotFound);

            order.OrderItems.Remove(item);

            Recalculate(order);

            await orderRepository.SaveChangesAsync(ct);

            logger.LogInformation("Item removed from order {OrderId}", orderId);
        }
    }
}
