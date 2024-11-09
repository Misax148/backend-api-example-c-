using MovieApi.Core.src.Application.DTO.User;

namespace MovieApi.Core.src.Domain.Interfaces.Services;

public interface IUserService
{
    Task<UserDto> CreateUserAsync(CreateUserDto userDto);
    Task<UserDto> UpdateUserAsync(Guid id, UpdateUserDto updateUserDto);
    Task<bool> DeleteUserAsync(Guid id);
    Task<UserDto> GetUserByIdAsync(Guid id);
    Task<UserDto> GetUserByEmailAsync(string email);
    Task<bool> IsEmailUniqueAsync(string email);
}
