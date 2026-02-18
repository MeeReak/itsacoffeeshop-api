using Azure.Core;
using kh.gov.nbc.bakong_khqr;
using kh.gov.nbc.bakong_khqr.model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using smakchet.application.Constants;
using smakchet.application.Constants.Enum;
using smakchet.application.DTOs.Category;
using smakchet.application.DTOs.Payment;
using smakchet.application.Exceptions;
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
        IMapper<Category, CategoryReadDto, CategoryDto, CategoryUpdateDto> mapper,
        ILogger<CategoryService> logger,
        IConfiguration configuration,
        HttpClient httpClient,
        IHttpContextAccessor contextAccessor) : IPaymentService
    {
        public async Task CheckStatus(int orderId, CancellationToken cancellationToken)
        {
            var payment = await paymentRepository.GetLatestPendingByOrderIdAsync(orderId, cancellationToken);
            if (payment == null)
            {
                logger.LogError(ErrorMessageConstants.ResourceNotFoundById, "Payment", orderId);
                throw new NotFoundException(string.Format(ErrorMessageConstants.ResourceNotFoundById, "Payment", orderId),
                    ErrorCodeConstants.NotFound);
            }

            var accessToken = configuration["Payment:AccessToken"];
            var url = $"{configuration["Payment:BakongUrl"]}/check_transaction_by_md5";
            var requestBody = new CheckTransactionRequest
            {
                Md5 = payment.ReferenceCode
            };
            var json = JsonConvert.SerializeObject(requestBody);
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync(cancellationToken);

            var responseContent = JsonConvert.DeserializeObject<BakongKHQRResponse>(responseString);

            if (responseContent!.ResponseMessage == "Success" || responseContent.ResponseCode == 0)
            {
                payment.Status = (int)PaymemtStatusEnum.Success;
                payment.PaidAt = DateTime.Now;
                await paymentRepository.SaveChangesAsync(cancellationToken);
            }
        }

        public Task<dynamic> DecodeKHQR()
        {
            throw new NotImplementedException();
        }

        public Task<dynamic> GenerateDeeplink()
        {
            throw new NotImplementedException();
        }

        public async Task<PaymentReadDto> GenerateKHQR(int orderId, CancellationToken cancellationToken)
        {
            var existing = await orderRepository.GetByIdAsync(orderId, cancellationToken);

            if (existing == null)
            {
                logger.LogError(ErrorMessageConstants.ResourceNotFoundById, "Order", orderId);
                throw new NotFoundException(string.Format(ErrorMessageConstants.ResourceNotFoundById, "Order", orderId),
                    ErrorCodeConstants.NotFound);
            }

            if (existing.Status != (int)OrderStatusEnum.Pending)
            {
                logger.LogError(ErrorMessageConstants.BadRequest);
                throw new NotFoundException(string.Format(ErrorMessageConstants.BadRequest),
                    ErrorCodeConstants.NotFound);
            }

            var KHQR = await GenerateQR(orderId, existing.Total);
            var payment = new Payment
            {
                CashierId = 2,
                Order = existing,
                Amount = existing.Total,
                ReferenceCode = KHQR.MD5.ToString(),
                Method = (int)PaidMethodEnum.Qr,
                Status = (int)PaymemtStatusEnum.Pending,
                CreatedAt = DateTime.Now
            };

            existing.Payments.Add(payment);
            await orderRepository.SaveChangesAsync(cancellationToken);

            return new PaymentReadDto
            {
                Id = payment.Id,
                Qr = KHQR.QR,
                ExpireAt = DateTime.Now + TimeSpan.FromMinutes(5)
            };
    }

        public Task<dynamic> Verification()
        {
            throw new NotImplementedException();
        }

        private async Task<dynamic> GenerateQR(int orderId, decimal total)
        {
            var bakongAccountId = configuration["Payment:BakongAccountID"];
            var merchantName = configuration["Payment:MerchantName"];
            var merchantCity = configuration["Payment:MerchantCity"];
            var mobileNumber = configuration["Payment:MobileNumber"];

            var response = BakongKHQR.GenerateIndividual(
                new IndividualInfo
                {
                    BakongAccountID = bakongAccountId,
                    MerchantName = merchantName,
                    MerchantCity = merchantCity,
                    MobileNumber = mobileNumber,
                    BillNumber = $"#{orderId}",
                    Currency = KHQRCurrency.KHR,
                    //AcquiringBank = "ABA Bank",
                    //Amount = (double)total,
                    Amount = 100,
                    ExpirationTimestamp = new DateTimeOffset(DateTime.Now.AddMinutes(5)).ToUnixTimeMilliseconds()
                });

            return response.Data;
        }

    }
}
