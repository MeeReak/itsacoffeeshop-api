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
        [ProducesResponseType(typeof(ResponseDto<ResponsePagingDto<ProductReadDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseDto<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPag([FromQuery] PaginationQueryParams param)
        {
            var products = await service.GetProductPagedAsync(param);
            return Ok(ResponseDto<ResponsePagingDto<ProductReadDto>>.Ok(products));
        }


        [HttpGet("{productId:int}")]
        [ProducesResponseType(typeof(ResponseDto<ProductReadDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseDto<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProduct(int productId, CancellationToken cancellationToken)
        {
            var product = await service.GetProductByIdAsync(productId, cancellationToken);
            return Ok(ResponseDto<ProductReadDto>.Ok(product));
        }


        [HttpPost]
        [ProducesResponseType(typeof(ResponseDto<ProductReadDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseDto<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SaveProduct([FromForm] ProductDto productDto, CancellationToken cancellationToken)
        {
            var product = await service.CreateProductAsync(productDto, cancellationToken);
            return StatusCode(StatusCodes.Status201Created, ResponseDto<ProductReadDto>.Ok(product, "Product created successfully"));
        }


        [HttpPut("{productId:int}")]
        [ProducesResponseType(typeof(ResponseDto<ProductReadDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseDto<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateProduct(int productId, [FromForm] ProductUpdateDto productDto, CancellationToken cancellationToken)
        {
            var product = await service.UpdateProductAsync(productId, productDto, cancellationToken);
            return Ok(ResponseDto<ProductReadDto>.Ok(product, "Product updated successfully"));
        }


        [HttpDelete("{productId:int}")]
        [ProducesResponseType(typeof(ResponseDto<object>), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseDto<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteProduct(int productId, CancellationToken cancellationToken)
        {
            await service.DeleteProductAsync(productId, cancellationToken);
            return Ok(ResponseDto<object>.Ok(null, "Product deleted successfully"));
        }
    }
}