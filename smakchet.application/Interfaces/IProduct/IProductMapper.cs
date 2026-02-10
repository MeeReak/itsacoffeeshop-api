using smakchet.application.DTOs.Product;
using smakchet.dal.Models;

namespace smakchet.application.Interfaces.IProduct
{
    public interface IProductMapper : IMapper<Product, ProductReadDto, ProductDto, ProductUpdateDto>
    {
    }
}
