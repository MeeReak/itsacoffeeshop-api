using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using smakchet.application.DTOs;
using smakchet.application.DTOs.Error;
using smakchet.application.DTOs.Success;
using smakchet.application.DTOs.User;
using smakchet.application.Interfaces.IUser;
using smakchet.dal.Models;

namespace smakchet.api.Controllers.V2026_01_01
{
    [ApiVersion("2026-01-01")]
    [Route("users")]
    [Produces("application/json")]
    [ApiController]
    public class UserController(IUserService service) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(ResponsePagingDto<UserReadDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseErrorDto), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPag([FromQuery] PaginationQueryParams param)
        {
            var user = await service.GetUsersPagedAsync(param);
            return StatusCode(StatusCodes.Status200OK, user);
        }


        [HttpGet("{userId:int}")]
        [ProducesResponseType(typeof(UserReadDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseErrorDto), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUser(int userId, CancellationToken cancellationToken)
        {

            var user = await service.GetUserByIdAsync(userId, cancellationToken);
            return StatusCode(StatusCodes.Status200OK, user);
        }


        [HttpPost]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseErrorDto), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SaveUser([FromBody] UserDto userDto, CancellationToken cancellationToken)
        {
            var user = await service.CreateUserAsync(userDto, cancellationToken);
            return StatusCode(StatusCodes.Status200OK, user);
        }


        [HttpPut("{userId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseErrorDto), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateUser(int userId, UserUpdateDto userDto, CancellationToken cancellationToken)
        {
            var user = await service.UpdateUserAsync(userId, userDto, cancellationToken);
            return StatusCode(StatusCodes.Status200OK, user);
        }


        [HttpDelete("{userId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseErrorDto), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteUser(int userId, CancellationToken cancellationToken)
        {
            await service.DeleteUserAsync(userId, cancellationToken);
            return StatusCode(StatusCodes.Status204NoContent);
        }
    }
}