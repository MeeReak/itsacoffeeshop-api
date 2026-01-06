using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using smakchet.application.Constants;
using smakchet.application.Constants.User;
using smakchet.application.DTOs;
using smakchet.application.DTOs.Success;
using smakchet.application.DTOs.User;
using smakchet.application.Exceptions;
using smakchet.application.Helpers;
using smakchet.application.Interfaces.IUser;
using smakchet.dal.Models;

namespace smakchet.application.Repositories;

public class UserRepository(SampleContext dbContext, IUserMapper mapper, IHttpContextAccessor contextAccessor)
    : IUserRepository
{
    public async Task<ResponsePagingDto<UserReadDto>> GetUsersPagedAsync(PaginationQueryParams param)
    {
        return await dbContext.Users
            .AsNoTracking()
            .OrderBy(u => u.Id)
            .ToPagedResultAsync(param.Skip, param.Top, mapper.ToReadDto, contextAccessor.HttpContext);
    }

    public async Task<UserReadDto?> GetUserByIdAsync(int userId)
    {
        var user = await dbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null)
            throw new NotFoundException(string.Format(ErrorMessageConstants.ResourceNotFoundById, "User", userId),
                ErrorCodeConstants.NotFound);
        var result = mapper.ToReadDto(user);
        return result;
    }

    public async Task<UserReadDto> UpdateUserAsync(int userId, UserDto user)
    {
        var existUser = await dbContext.Users.FindAsync(userId);
        if (existUser == null)
            throw new NotFoundException(string.Format(ErrorMessageConstants.ResourceNotFoundById, "User", userId),
                ErrorCodeConstants.NotFound);

        mapper.UpdateEntity(existUser, user);

        dbContext.Users.Update(existUser);
        await dbContext.SaveChangesAsync();
        var response = mapper.ToReadDto(existUser);
        return response;
    }

    public async Task<UserReadDto> CreateUserAsync(UserDto user)
    {
        if (string.IsNullOrEmpty(user.Name))
        {
            throw new BadRequestException(UserMessageConstant.RequiredName, ErrorCodeConstants.InvalidInput);
        }

        var userEntity = mapper.ToEntity(user);
        await dbContext.Users.AddAsync(userEntity);
        await dbContext.SaveChangesAsync();
        var response = mapper.ToReadDto(userEntity);
        return response;
    }

    public async Task DeleteUserAsync(int userId)
    {
        var user = await dbContext.Users.FindAsync(userId);
        //if (user == null)
        //    throw new NotFoundException(string.Format(ErrorMessageConstants.ResourceNotFoundById,"User",userId), ErrorCodeConstants.NotFound);
        if (user != null)
        {
            dbContext.Users.Remove(user);
            await dbContext.SaveChangesAsync();
        }
    }
}