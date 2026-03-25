using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using smakchet.application.DTOs;
using smakchet.application.DTOs.Category;
using smakchet.application.DTOs.Error;
using smakchet.application.DTOs.Success;
using smakchet.application.Interfaces.ICategory;

namespace smakchet.api.Controllers.V2026_01_01
{
    [ApiVersion("2026-01-01")]
    [Route("categories")]
    [Produces("application/json")]
    [ApiController]
    public class CategoryController(ICategoryService service) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(ResponsePagingDto<CategoryReadDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseErrorDto), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPag([FromQuery] PaginationQueryParams param)
        {
            var categories = await service.GetCategoryPagedAsync(param);
            return StatusCode(StatusCodes.Status200OK, categories);
        }


        [HttpGet("{categoryId:int}")]
        [ProducesResponseType(typeof(CategoryReadDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseErrorDto), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCategory(int categoryId, CancellationToken cancellationToken)
        {

            var category = await service.GetCategoryByIdAsync(categoryId, cancellationToken);
            return StatusCode(StatusCodes.Status200OK, category);
        }


        [HttpPost]
        [ProducesResponseType(typeof(CategoryDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseErrorDto), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SaveCategory([FromBody] CategoryDto categoryDto, CancellationToken cancellationToken)
        {
            var category = await service.CreateCategoryAsync(categoryDto, cancellationToken);
            return StatusCode(StatusCodes.Status201Created, category);
        }


        [HttpPut("{categoryId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseErrorDto), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateCategory(int categoryId, CategoryUpdateDto categoryDto, CancellationToken cancellationToken)
        {
            var category = await service.UpdateCategoryAsync(categoryId, categoryDto, cancellationToken);
            return StatusCode(StatusCodes.Status201Created, category);
        }


        [HttpDelete("{categoryId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseErrorDto), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCategory(int categoryId, CancellationToken cancellationToken)
        {
            await service.DeleteCategoryAsync(categoryId, cancellationToken);
            return StatusCode(StatusCodes.Status204NoContent);
        }
    }
}