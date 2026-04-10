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
        [ProducesResponseType(typeof(ResponseDto<ResponsePagingDto<UserReadDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseDto<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPag([FromQuery] PaginationQueryParams param)
        {
            var user = await service.GetUsersPagedAsync(param);
            return Ok(ResponseDto<ResponsePagingDto<UserReadDto>>.Ok(user));
        }


        [HttpGet("{userId:int}")]
        [ProducesResponseType(typeof(ResponseDto<UserReadDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseDto<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUser(int userId, CancellationToken cancellationToken)
        {

            var user = await service.GetUserByIdAsync(userId, cancellationToken);
            return Ok(ResponseDto<UserReadDto>.Ok(user));
        }


        [HttpPost]
        [ProducesResponseType(typeof(ResponseDto<UserReadDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseDto<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SaveUser([FromBody] UserDto userDto, CancellationToken cancellationToken)
        {
            var user = await service.CreateUserAsync(userDto, cancellationToken);
            return StatusCode(StatusCodes.Status201Created, ResponseDto<UserReadDto>.Ok(user, "User created successfully"));
        }


        [HttpPut("{userId:int}")]
        [ProducesResponseType(typeof(ResponseDto<UserReadDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseDto<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateUser(int userId, UserUpdateDto userDto, CancellationToken cancellationToken)
        {
            var user = await service.UpdateUserAsync(userId, userDto, cancellationToken);
            return Ok(ResponseDto<UserReadDto>.Ok(user, "User updated successfully"));
        }


        [HttpDelete("{userId:int}")]
        [ProducesResponseType(typeof(ResponseDto<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseDto<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteUser(int userId, CancellationToken cancellationToken)
        {
            await service.DeleteUserAsync(userId, cancellationToken);
            return Ok(ResponseDto<object>.Ok(null, "User deleted successfully"));
        }
    }
}