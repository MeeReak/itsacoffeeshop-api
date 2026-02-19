using smakchet.application.DTOs.Order;
using smakchet.application.DTOs.Payment;
using smakchet.application.Interfaces.IPayment;
using smakchet.dal.Models;

namespace smakchet.application.Mappings
{
    public class PaymentMapper : IPaymentMapper
    {
        public Payment ToEntity(PaymentDto dto)
        {
            return new Payment();
        }

        public PaymentReadDto ToReadDto(Payment entity)
        {
            return new PaymentReadDto
            {
                Id = entity.Id,
                Status = entity.Status,
                Amount = entity.Amount,
                Method = entity.Method,
                ReferenceCode = entity.ReferenceCode,
                CreatedAt = entity.CreatedAt,
                ExpireAt = entity.ExpiredAt,
                PaidAt = entity.PaidAt,
                Orders = new OrderReadDto
                {
                    Id = entity.Order.Id,
                    Status = entity.Order.Status,
                    Type = entity.Order.Type,
                    Subtotal = entity.Order.Subtotal,
                    Tax = entity.Order.Tax,
                    Total = entity.Order.Total,
                    Number = entity.Order.Number,
                    CreatedAt = entity.Order.CreatedAt
                }
            };
        }

        public void UpdateEntity(Payment entity, PaymentUpdateDto dto)
        {
            entity.Method = dto.Method;
        }
    }
}
