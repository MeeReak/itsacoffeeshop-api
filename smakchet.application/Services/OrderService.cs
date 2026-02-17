using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using smakchet.application.Constants;
using smakchet.application.Constants.Enum;
using smakchet.application.Constants.Order;
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

namespace smakchet.application.Services;

public class OrderService(
    IOrderRepository orderRepository,
    IProductRepository productRepository,
    IMapper<Order, OrderReadDto, OrderDto, OrderUpdateDto> orderMapper,
    ILogger<OrderService> logger,
    IHttpContextAccessor contextAccessor) : IOrderService
{
    public async Task<OrderReadDto> CreateOrderAsync(OrderDto orderDto, CancellationToken cancellationToken)
    {
        try
        {
            var order = new Order
            {
                Type = (int)orderDto.Type,
                Status = (int)OrderStatusEnum.Pending,
                Subtotal = 0,
                Tax = 0,
                Total = 0,
                CashierId = orderDto.CashierId
            };
            await orderRepository.AddAsync(order, cancellationToken);
            await orderRepository.SaveChangesAsync(cancellationToken);

            order.Number = $"ORD-{order.Id:D6}";
            var orderStatusLog = new OrderStatusLog
            {
                Order = order,
                OldStatus = order.Status,
                CashierId = order.CashierId
            };

            order.OrderStatusLogs.Add(orderStatusLog);
            await orderRepository.SaveChangesAsync(cancellationToken);

            return orderMapper.ToReadDto(order);
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
            .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(param.Search))
                query = query.Where(o => o.Number == param.Search);

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
            await orderRepository.SaveChangesAsync(cancellationToken);
            logger.LogInformation(SuccessMessageConstants.Updated, "Order");
            return orderMapper.ToReadDto(existing);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ErrorMessageConstants.OperationFailed, "UpdateOrder");
            throw;
        }
    }


    private void Recalculate(Order order)
    {
        decimal subtotal = 0;

        foreach (var item in order.OrderItems)
        {
            var unitPrice = item.Size switch
            {
                (int)OrderItemSizeEnum.Small => item.Price,
                (int)OrderItemSizeEnum.Medium => item.Price + 1m,
                (int)OrderItemSizeEnum.Large => item.Price + 2m,
                _ => item.Price
            };

            subtotal += unitPrice * item.Quantity;
        }

        decimal taxRate = 0;
        var tax = subtotal * taxRate;

        order.Subtotal = subtotal;
        order.Total = subtotal + tax;
        order.UpdatedAt = DateTime.UtcNow;
    }


    public async Task<OrderReadDto?> GetOrderItemByIdAsync(int orderId, CancellationToken cancellationToken)
    {
        var existing = await orderRepository.GetOrderWithItemsAsync(orderId, cancellationToken);
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
    

    public async Task AddItemAsync(int orderId, OrderItemDto itemDto, CancellationToken ct)
    {
        var product = await productRepository.GetByIdAsync(itemDto.ProductId, ct);
        if (product == null)
            throw new NotFoundException("Product not found");

        var order = await orderRepository.GetOrderWithItemsAsync(orderId, ct);
        if (order == null)
            throw new NotFoundException("Order not found");

        if (order.Status != (int)OrderStatusEnum.Pending)
            throw new BadRequestException("Order is not open");

        var existingItem = order.OrderItems
            .FirstOrDefault(i => i.ProductId == itemDto.ProductId
                                 && i.Size == (int)itemDto.Size);

        if (existingItem != null)
        {
            existingItem.Quantity += itemDto.Quantity;
        }
        else
        {
            var newItem = new OrderItem
            {
                ProductId = product.Id,
                ProductName = product.Name,
                Price = product.Price, 
                Quantity = itemDto.Quantity,
                Size = (int)itemDto.Size,
                Note = itemDto.Note,
                CreatedAt = DateTime.UtcNow
            };

            order.OrderItems.Add(newItem);
        }

        Recalculate(order);

        await orderRepository.SaveChangesAsync(ct);
    }

    
    public async Task RemoveItemAsync(int orderId, int itemId, CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetOrderWithItemsAsync(orderId, cancellationToken);
        if (order == null)
            throw new NotFoundException(
                string.Format(ErrorMessageConstants.ResourceNotFoundById, "Order", orderId),
                ErrorCodeConstants.NotFound);

        if (order.Status != (int)OrderStatusEnum.Pending)
            throw new BadRequestException(OrderMessageConstant.NotOpen);

        var item = order.OrderItems.FirstOrDefault(i => i.Id == itemId);
        if (item == null)
            throw new NotFoundException(
                string.Format(ErrorMessageConstants.ResourceNotFoundById, "Item", itemId),
                ErrorCodeConstants.NotFound);

        order.OrderItems.Remove(item);

        Recalculate(order);

        await orderRepository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Item removed from order {OrderId}", orderId);
    }


    public async Task UpdateItemAsync(int orderId, int itemId, OrderItemUpdateDto itemDto, CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetOrderWithItemsAsync(orderId, cancellationToken);
        if (order == null)
        {
            logger.LogError(ErrorMessageConstants.ResourceNotFoundById, "Order", orderId);
            throw new NotFoundException(
                string.Format(ErrorMessageConstants.ResourceNotFoundById, "Order", orderId),
                ErrorCodeConstants.NotFound);
        }

        if (order.Status != (int)OrderStatusEnum.Pending)
            throw new BadRequestException(OrderMessageConstant.NotOpen);

        var item = order.OrderItems.FirstOrDefault(i => i.Id == itemId);
        if (item == null)
        {
            logger.LogError(ErrorMessageConstants.ResourceNotFoundById, "Item", itemId);
            throw new NotFoundException(
                string.Format(ErrorMessageConstants.ResourceNotFoundById, "Item", itemId),
                ErrorCodeConstants.NotFound);
        }

        if (itemDto.Quantity <= 0)
            throw new BadRequestException("Quantity must be greater than 0");

        item.Quantity = itemDto.Quantity;
        item.Note = itemDto.Note;
        item.Size = (int)itemDto.Size;

        Recalculate(order);

        await orderRepository.SaveChangesAsync(cancellationToken);
        logger.LogInformation("Item {ItemId} updated in order {OrderId}", itemId, orderId);
    }


    public async Task<OrderReadDto?> UpdateStatueOrderAsync(int orderId, OrderStatusDto statusDto, CancellationToken cancellationToken)
    {
        var existing = await orderRepository.GetByIdAsync(orderId, cancellationToken);

        if (existing == null)
        {
            logger.LogError(ErrorMessageConstants.ResourceNotFoundById, "Order", orderId);
            throw new NotFoundException(
                string.Format(ErrorMessageConstants.ResourceNotFoundById, "Order", orderId),
                ErrorCodeConstants.NotFound);
        }

        try
        {
            var newStatus = (int)statusDto.Status;
            if (existing.Status == newStatus)
                return orderMapper.ToReadDto(existing);

            var oldStatus = existing.Status;

            existing.Status = newStatus;

            var statusLog = new OrderStatusLog
            {
                OrderId = existing.Id,
                OldStatus = oldStatus,
                NewStatus = newStatus,
                ChangedBy = existing.CashierId,
                CashierId = existing.CashierId,
                UpdatedAt = DateTime.Now
            };

            existing.OrderStatusLogs.Add(statusLog);

            await orderRepository.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Order {OrderId} status updated from {Old} to {New}",
                orderId, oldStatus, newStatus);

            return orderMapper.ToReadDto(existing);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ErrorMessageConstants.OperationFailed, "UpdateStatueOrder");
            throw;
        }
    }
}