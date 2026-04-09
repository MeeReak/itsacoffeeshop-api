using kh.gov.nbc.bakong_khqr;
using kh.gov.nbc.bakong_khqr.model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using smakchet.application.Constants;
using smakchet.application.Constants.Enum;
using smakchet.application.DTOs.Payment;
using smakchet.application.Exceptions;
using smakchet.application.Helpers;
using smakchet.application.Interfaces;
using smakchet.application.Interfaces.IOrder;
using smakchet.application.Interfaces.IPayment;
using smakchet.dal.Models;
using System.Net.Http.Headers;
using System.Text;

namespace smakchet.application.Services
{
  public class PaymentService(
      IOrderRepository orderRepository,
      IPaymentRepository paymentRepository,
      IUnitOfWork unitOfWork,
      IMapper<Payment, PaymentReadDto, PaymentDto, PaymentUpdateDto> mapper,
      ILogger<PaymentService> logger,
      IConfiguration configuration,
      HttpClient httpClient,
      IBackgroundQueueService<PaymentStatusJob> backgroundQueue
  ) : IPaymentService
  {
    public async Task<PaymentStatusResponseDto> CheckStatusAsync(
    int paymentId,
    CancellationToken cancellationToken)
    {
      var payment = await paymentRepository.GetByIdAsync(paymentId, cancellationToken)
          ?? throw new NotFoundException(
              string.Format(ErrorMessageConstants.ResourceNotFoundById, "Payment", paymentId),
              ErrorCodeConstants.NotFound);

      // Already finished
      if (payment.Status == (int)PaymemtStatusEnum.Success ||
          payment.Status == (int)PaymemtStatusEnum.Failed)
      {
        return MapResponse(payment);
      }

      var response = await CheckTransactionWithBakong(payment.ReferenceCode, cancellationToken);

      if (response.ResponseCode == 0)
      {
        payment.Status = (int)PaymemtStatusEnum.Success;
        payment.PaidAt = DateTime.UtcNow;
      }
      else if (payment.ExpiredAt < DateTime.UtcNow)
      {
        payment.Status = (int)PaymemtStatusEnum.Failed;
      }

      await unitOfWork.SaveChangesAsync(cancellationToken);

      logger.LogInformation(
          "Payment status checked for PaymentId {PaymentId}, Status {Status}",
          payment.Id,
          payment.Status);

      return MapResponse(payment);
    }

    //public Task<dynamic> DecodeKHQR()
    //{
    //    throw new NotImplementedException();
    //}


    //public Task<dynamic> GenerateDeeplink()
    //{
    //    throw new NotImplementedException();
    //}


    public async Task<PaymentCheckOutDto> CheckOutAsync(int orderId, PaymentDto paymentDto, CancellationToken cancellationToken)
    {
      try
      {
        var existingPayment = await paymentRepository.GetLatestPendingByOrderIdAsync(orderId, cancellationToken);
        if (existingPayment != null && existingPayment.ExpiredAt > DateTime.UtcNow)
        {
          logger.LogInformation("Existing pending payment found for OrderId {OrderId}, PaymentId {PaymentId}", orderId, existingPayment.Id);
          return new PaymentCheckOutDto
          {
            Id = existingPayment.Id,
            Qr = null!, // QR code is not stored, so we cannot return it here
            Amount = existingPayment.Amount,
            Currency = 0, // Currency is not stored in Payment entity, so we cannot return it here
            Method = existingPayment.Method,
            ExpireAt = existingPayment.ExpiredAt
          };
        }

        var order = await orderRepository.GetByIdAsync(orderId, cancellationToken)
                  ?? throw new NotFoundException(
                      string.Format(ErrorMessageConstants.ResourceNotFoundById, "Order", orderId),
                      ErrorCodeConstants.NotFound);

        if (order.Status != (int)OrderStatusEnum.Pending)
          throw new BadRequestException("Order is not in pending status.");

        var qrResult = GenerateQr(orderId, order.Total, paymentDto.Currency);

        var payment = new Payment
        {
          Order = order,
          Amount = order.Total,
          ReferenceCode = qrResult.Md5,
          Method = (int)paymentDto.Method!,
          Status = (int)PaymemtStatusEnum.Pending,
          CreatedAt = DateTime.UtcNow,
          ExpiredAt = DateTime.UtcNow.AddMinutes(5),
          CashierId = 2
        };

        await paymentRepository.AddAsync(payment, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        // await backgroundQueue.Enqueue(new PaymentStatusJob(payment.Id));

        return new PaymentCheckOutDto
        {
          Id = payment.Id,
          Qr = qrResult.Qr,
          Amount = payment.Amount,
          Currency = (int)paymentDto.Currency!,
          Method = payment.Method,
          ExpireAt = DateTime.UtcNow.AddMinutes(5)
        };
      }
      catch (System.Exception ex)
      {
        logger.LogError(ex, ErrorMessageConstants.OperationFailed, "CheckOutAsync");
        throw;
      }
    }


    //public Task<dynamic> Verification()
    //{
    //    throw new NotImplementedException();
    //}


    private BakongKHQRData GenerateQr(int orderId, decimal total, KHQRCurrencyEnum? type)
    {
      try
      {
        var currency = type == KHQRCurrencyEnum.KHR ? KHQRCurrency.KHR : KHQRCurrency.USD;
        var response = BakongKHQR.GenerateIndividual(
            new IndividualInfo
            {
              BakongAccountID = configuration["Payment:BakongAccountID"],
              MerchantName = configuration["Payment:MerchantName"],
              MerchantCity = configuration["Payment:MerchantCity"],
              MobileNumber = configuration["Payment:MobileNumber"],
              BillNumber = $"#{orderId}",
              Currency = KHQRCurrency.USD,
              Amount = (double)total,
              ExpirationTimestamp = DateTimeOffset.UtcNow.AddMinutes(5).ToUnixTimeMilliseconds()
            });

        return new BakongKHQRData
        {
          Qr = response.Data.QR,
          Md5 = response.Data.MD5
        };
      }
      catch (System.Exception ex)
      {
        logger.LogError(ex, ErrorMessageConstants.OperationFailed, "GenerateQr");
        throw new Exception("Failed to generate QR code.");
      }
    }


    private async Task<BakongKHQRResponse> CheckTransactionWithBakong(
        string md5,
        CancellationToken cancellationToken)
    {
      try
      {
        var accessToken = configuration["Payment:AccessToken"];
        var url = $"{configuration["Payment:BakongUrl"]}/check_transaction_by_md5";
        var requestBody = new CheckTransactionRequest
        {
          md5 = md5
        };
        var json = JsonConvert.SerializeObject(requestBody);
        var request = new HttpRequestMessage(HttpMethod.Post, url)
        {
          Content = new StringContent(json, Encoding.UTF8, "application/json")
        };

        request.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", accessToken);

        var response = await httpClient.SendAsync(request, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
          logger.LogError("Bakong API call failed: {StatusCode}", response.StatusCode);
          throw new Exception("Failed to check transaction with Bakong.");
        }

        var content = await response.Content.ReadAsStringAsync(cancellationToken);

        return JsonConvert.DeserializeObject<BakongKHQRResponse>(content)!;
      }
      catch (Exception ex)
      {
        logger.LogError(ex, ErrorMessageConstants.OperationFailed, "CheckTransactionWithBakong");
        throw;
      }
    }



    public async Task<PaymentReadDto?> GetPaymentOrderByIdAsync(int paymentId, CancellationToken cancellationToken)
    {
      var payment = await paymentRepository.GetPaymentOrderByIdAsync(paymentId, cancellationToken)
                    ?? throw new NotFoundException(
                        string.Format(ErrorMessageConstants.ResourceNotFoundById, "Order", paymentId),
                        ErrorCodeConstants.NotFound);

      return mapper.ToReadDto(payment);
    }

    private static PaymentStatusResponseDto MapResponse(Payment payment)
    {
      var status = (PaymemtStatusEnum)payment.Status;

      return new PaymentStatusResponseDto
      {
        PaymentId = payment.Id,
        ReferenceCode = payment.ReferenceCode,
        Status = status,
        PaidAt = payment.PaidAt,
        ExpiredAt = payment.ExpiredAt,
        Message = status switch
        {
          PaymemtStatusEnum.Success => "Payment completed successfully",
          PaymemtStatusEnum.Failed => "Payment expired or failed",
          PaymemtStatusEnum.Pending => "Waiting for payment",
          _ => "Unknown payment status"
        }
      };
    }
  }
}
