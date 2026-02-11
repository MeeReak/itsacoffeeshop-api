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
using smakchet.dal.Models;

namespace smakchet.application.Services
{
    public class OrderService(
        IOrderRepository repository,
        IMapper<Order, OrderReadDto, OrderDto, OrderUpdateDto> orderMapper,
        IMapper<OrderItem, OrderItemReadDto, OrderItemDto, OrderItemUpdateDto> orderItemMapper,
        ILogger<OrderService> logger,
        IHttpContextAccessor contextAccessor) : IOrderService
    {
        private void Recalculate(Order order, OrderItemSizeEnum size)
        {
            switch (size)
            {
                case OrderItemSizeEnum.Small:
                    // apply small logic
                    break;

                case OrderItemSizeEnum.Medium:
                    // apply medium logic
                    break;

                case OrderItemSizeEnum.Large:
                    // apply large logic
                    break;
            }

            order.Subtotal = order.OrderItems.Sum(i => i.Price * i.Quantity);
            order.Total = order.Subtotal;
            order.UpdatedAt = DateTime.UtcNow;
        }





        public async Task<OrderReadDto> CreateOrderAsync(OrderDto orderDto, CancellationToken cancellationToken)
        {
            var existing = await repository.Query()
                .Where(o => o.Number == orderDto.Number
                            && o.CreatedAt!.Value.Date == DateTime.Today)
                .FirstOrDefaultAsync(cancellationToken);
            if (existing != null)
            {
                logger.LogError(ErrorMessageConstants.AlreadyExists, "Order", orderDto.Number);
                throw new DuplicateException(
                    string.Format(ErrorMessageConstants.AlreadyExists, "Order", orderDto.Number),
                    ErrorCodeConstants.Conflict);
            }

            try
            {
                var mapped = orderMapper.ToEntity(orderDto);
                await repository.AddAsync(mapped, cancellationToken);
                await repository.SaveChangesAsync(cancellationToken);
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
            var existing = await repository.GetByIdAsync(orderId, cancellationToken);
            if (existing == null)
            {
                logger.LogError(ErrorMessageConstants.ResourceNotFoundById, "Order", orderId);
                throw new NotFoundException(string.Format(ErrorMessageConstants.ResourceNotFoundById, "Order", orderId),
                    ErrorCodeConstants.NotFound);
            }
          
            try
            {
                await repository.DeleteAsync(existing, cancellationToken);
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
            var existing = await repository.GetByIdAsync(orderId, cancellationToken);
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
            IQueryable<Order> query = repository.Query()
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
            var existing = await repository.GetByIdAsync(orderId, cancellationToken);
            if (existing == null)
            {
                logger.LogError(ErrorMessageConstants.ResourceNotFoundById, "Order", orderId);
                throw new NotFoundException(string.Format(ErrorMessageConstants.ResourceNotFoundById, "Order", orderId),
                    ErrorCodeConstants.NotFound);
            }

            try
            {
                orderMapper.UpdateEntity(existing, orderDto);
                await repository.UpdateAsync(existing, cancellationToken);
                logger.LogInformation(SuccessMessageConstants.Updated, "Order");
                return orderMapper.ToReadDto(existing);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ErrorMessageConstants.OperationFailed, "UpdateOrder");
                throw;
            }
        }



        public async Task AddItemAsync(int orderId, OrderItemDto itemDto, CancellationToken cancellationToken)
        {
            var order = await repository.GetOrderWithItemsAsync(orderId, cancellationToken);
            if (order == null)
            {
                logger.LogError(ErrorMessageConstants.ResourceNotFoundById, "Order", orderId);
                throw new NotFoundException(string.Format(ErrorMessageConstants.ResourceNotFoundById, "Order", orderId),
                    ErrorCodeConstants.NotFound);
            }

            if (order.Status != OrderStatusEnum.Pending.ToString())
                throw new BadRequestException("Order is not open");


            var existingItem = order.OrderItems
                .FirstOrDefault(i => i.ProductId == itemDto.ProductId && i.Size == itemDto.Size);

            if (existingItem != null)
            {
                existingItem.Quantity += itemDto.Quantity;
            }
            else
            {
                var orderItem = orderItemMapper.ToEntity(itemDto);
                order.OrderItems.Add(orderItem);
            }

            Recalculate(order);

            await repository.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Item added to order {OrderId}", orderId);
        }
    }
}
