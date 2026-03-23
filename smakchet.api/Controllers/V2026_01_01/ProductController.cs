using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using smakchet.application.DTOs;
using smakchet.application.DTOs.Product;
using smakchet.application.DTOs.Error;
using smakchet.application.DTOs.Success;
using smakchet.application.Interfaces.IProduct;

namespace smakchet.api.Controllers.V2026_01_01
{
    [ApiVersion("2026-01-01")]
    [Route("products")]
    [Produces("application/json")]
    [ApiController]
    public class ProductController(IProductService service) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(ResponsePagingDto<ProductReadDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseErrorDto), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPag([FromQuery] PaginationQueryParams param)
        {
            var categories = await service.GetProductPagedAsync(param);
            return StatusCode(StatusCodes.Status200OK, categories);
        }


        [HttpGet("{productId:int}")]
        [ProducesResponseType(typeof(ProductReadDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseErrorDto), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProduct(int productId, CancellationToken cancellationToken)
        {

            var product = await service.GetProductByIdAsync(productId, cancellationToken);
            return StatusCode(StatusCodes.Status200OK, product);
        }


        [HttpPost]
        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseErrorDto), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SaveProduct([FromForm] ProductDto productDto, CancellationToken cancellationToken)
        {
            var product = await service.CreateProductAsync(productDto, cancellationToken);
            return StatusCode(StatusCodes.Status201Created, product);
        }


        [HttpPut("{productId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseErrorDto), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateProduct(int productId, ProductUpdateDto productDto, CancellationToken cancellationToken)
        {
            var product = await service.UpdateProductAsync(productId, productDto, cancellationToken);
            return StatusCode(StatusCodes.Status201Created, product);
        }


        [HttpDelete("{ProductId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseErrorDto), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteProduct(int productId, CancellationToken cancellationToken)
        {
            await service.DeleteProductAsync(productId, cancellationToken);
            return StatusCode(StatusCodes.Status204NoContent);
        }
    }
}