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
using smakchet.application.Interfaces.IOrderItem;
using smakchet.application.Interfaces.IPayment;
using smakchet.application.Interfaces.IProduct;
using smakchet.dal.Models;

namespace smakchet.application.Services;

public class OrderService(
    IOrderRepository orderRepository,
    IOrderItemRepository orderItemRepository,
    IProductRepository productRepository,
    IPaymentRepository paymentRepository,
    IUnitOfWork unitOfWork,
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
                Type = (int)orderDto.Type!,
                Status = (int)OrderStatusEnum.Pending,
                Subtotal = 0,
                Tax = 0,
                Total = 0,
                CashierId = orderDto.CashierId
            };
            await orderRepository.AddAsync(order, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            foreach (var item in orderDto.OrderItems)
            {
                var product = await productRepository.GetByIdAsync(item.ProductId, cancellationToken);
                if (product == null) throw new NotFoundException("Product not found");

                var orderItem = new OrderItem
                {
                    Number = item.Number,
                    OrderId = order.Id,      
                    ProductId = product.Id,
                    ProductName = product.Name,
                    Note = item.Note,
                    Price = product.Price,
                    Quantity = item.Quantity,
                    //Size = (int)item.Size!
                };

                await orderItemRepository.AddAsync(orderItem, cancellationToken);
            }
            await unitOfWork.SaveChangesAsync(cancellationToken);
            order.Number = $"ORD-{order.Id:D6}";
            var orderStatusLog = new OrderStatusLog
            {
                Order = order,
                OldStatus = order.Status,
                CashierId = order.CashierId
            };

            order.OrderStatusLogs.Add(orderStatusLog);
            Recalculate(order);
            await unitOfWork.SaveChangesAsync(cancellationToken);

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
            orderRepository.Update(existing);
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


    public async Task<OrderReadDto?> UpdateOrderAsync(int orderId, OrderUpdateDto payload,
    CancellationToken cancellationToken)
    {
        var existing = await orderRepository.GetOrderWithItemsAsync(orderId, cancellationToken);
        if (existing == null)
            throw new NotFoundException(
                string.Format(ErrorMessageConstants.ResourceNotFoundById, "Order", orderId),
                ErrorCodeConstants.NotFound);

        if (existing.Status != (int)OrderStatusEnum.Pending)
            throw new BadRequestException(OrderMessageConstant.NotOpen);

        try
        {
            // Update order header
            existing.Type = (int)payload.Type!;
            existing.CashierId = payload.CashierId;

            // --- Update or Add Order Items ---
            foreach (var dtoItem in payload.OrderItems)
            {

                var exitedProduct = await productRepository.GetByIdAsync(dtoItem.ProductId, cancellationToken);
                if (exitedProduct == null)
                    throw new NotFoundException(
                        string.Format(ErrorMessageConstants.ResourceNotFoundById, "Product", dtoItem.ProductId),
                        ErrorCodeConstants.NotFound);

                var exitedSize = await productRepository.GetByIdAsync(dtoItem.SizeId, cancellationToken);
                if (exitedProduct == null)
                    throw new NotFoundException(
                        string.Format(ErrorMessageConstants.ResourceNotFoundById, "Product", dtoItem.ProductId),
                        ErrorCodeConstants.NotFound);

                // Match by ProductId + Number
                var item = existing.OrderItems
                    .FirstOrDefault(i => i.ProductId == dtoItem.ProductId && i.Number == dtoItem.Number);

                if (item == null)
                {
                    // Add new item if quantity > 0
                    if (dtoItem.Quantity <= 0) continue;

                    item = new OrderItem
                    {
                        ProductId = exitedProduct.Id,
                        ProductName = exitedProduct.Name,
                        Quantity = dtoItem.Quantity,
                        Note = dtoItem.Note,
                        //Size = exitedSize.Id,
                        Number = dtoItem.Number,
                        Price = exitedProduct.Price
                    };
                    existing.OrderItems.Add(item);

                    logger.LogInformation("Item {ProductId}-{Number} added to order {OrderId}", dtoItem.ProductId, dtoItem.Number, orderId);
                }
                else
                {
                    // Update existing item
                    if (dtoItem.Quantity <= 0)
                    {
                        existing.OrderItems.Remove(item);
                        logger.LogInformation("Item {ProductId}-{Number} removed from order {OrderId}", dtoItem.ProductId, dtoItem.Number, orderId);
                        continue;
                    }

                    item.Quantity = dtoItem.Quantity;
                    item.Note = dtoItem.Note;
                    //item.Size = (int)dtoItem.Size;
                    item.Number = dtoItem.Number;

                    logger.LogInformation("Item {ProductId}-{Number} updated in order {OrderId}", dtoItem.ProductId, dtoItem.Number, orderId);
                }
            }

            // --- Remove items not in the payload ---
            foreach (var item in existing.OrderItems.ToList())
            {
                if (!payload.OrderItems.Any(d => d.ProductId == item.ProductId && d.Number == item.Number))
                {
                    existing.OrderItems.Remove(item);
                    logger.LogInformation("Item {ProductId}-{Number} removed from order {OrderId}", item.ProductId, item.Number, orderId);
                }
            }

            // Recalculate totals
            Recalculate(existing);

            orderRepository.Update(existing);
            await unitOfWork.SaveChangesAsync(cancellationToken);

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
            //var unitPrice = item.Size switch
            //{
            //    OrderItemSizeEnum.Small => item.Price,
            //    OrderItemSizeEnum.Medium => item.Price + 1m,
            //    _ => item.Price
            //};

            //subtotal += unitPrice * item.Quantity;
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


    public async Task<OrderReadDto?> GetStatusOrderAsync(int orderId, CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetByIdAsync(orderId, cancellationToken)
                    ?? throw new NotFoundException(
                        string.Format(ErrorMessageConstants.ResourceNotFoundById, "Order", orderId),
                        ErrorCodeConstants.NotFound);

        var payment = await paymentRepository.GetLatestPendingByOrderIdAsync(orderId, cancellationToken)
                      ?? throw new NotFoundException(
                          string.Format(ErrorMessageConstants.ResourceNotFoundById, "Payment", orderId),
                          ErrorCodeConstants.NotFound);

        try
        {
            int? newStatus = payment.Status switch
            {
                (int)PaymemtStatusEnum.Success => (int)OrderStatusEnum.Paid,
                (int)PaymemtStatusEnum.Failed => (int)OrderStatusEnum.Canceled,
                _ => null
            };

            if (newStatus == null || order.Status == newStatus)
                return orderMapper.ToReadDto(order);

            var oldStatus = order.Status;
            order.Status = newStatus.Value;

            order.OrderStatusLogs.Add(new OrderStatusLog
            {
                OrderId = order.Id,
                OldStatus = oldStatus,
                NewStatus = newStatus.Value,
                ChangedBy = order.CashierId,
                CashierId = order.CashierId,
                UpdatedAt = DateTime.Now
            });

            await unitOfWork.SaveChangesAsync(cancellationToken);

            logger.LogInformation(
                "Order {OrderId} status updated from {Old} to {New}",
                orderId, oldStatus, newStatus.Value);

            return orderMapper.ToReadDto(order);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ErrorMessageConstants.OperationFailed, "GetStatusOrderAsync");
            throw;
        }
    }
}