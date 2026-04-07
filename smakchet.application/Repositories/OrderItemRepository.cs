using smakchet.application.Interfaces.IOrderItem;
using smakchet.dal.Models;

namespace smakchet.application.Repositories;

public class OrderItemRepository(SmakchetContext context) : BaseRepository<OrderItem>(context), IOrderItemRepository
{

}