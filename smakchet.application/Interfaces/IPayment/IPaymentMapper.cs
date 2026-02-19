using smakchet.application.DTOs.Payment;
using smakchet.dal.Models;

namespace smakchet.application.Interfaces.IPayment
{
    public interface IPaymentMapper : IMapper<Payment, PaymentReadDto, PaymentDto, PaymentUpdateDto>
    {
    }
}
