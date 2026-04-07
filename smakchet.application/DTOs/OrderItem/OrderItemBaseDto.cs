using smakchet.application.Constants.Order;
using System.ComponentModel.DataAnnotations;

namespace smakchet.application.DTOs.OrderItem
{
    public class OrderItemBaseDto
    {
        [Required(ErrorMessage = OrderMessageConstant.RequiredProductId)]
        public int ProductId { get; set; }


        [Required(ErrorMessage = OrderMessageConstant.RequiredQuantity)]
        [RegularExpression(@"^\d+$", ErrorMessage = OrderMessageConstant.InvalidQuantity)]
        public int Quantity { get; set; }

        public int? SizeId { get; set; }
        public int? SugarLevelId { get; set; }
        public int? IceLevelId { get; set; }
        public int? CoffeeLevelId { get; set; }
        public int? VariationId { get; set; }
        public string? Note { get; set; }

        [Required(ErrorMessage = OrderMessageConstant.RequiredNumber)]
        public string Number { get; set; } = string.Empty;
    }

}
