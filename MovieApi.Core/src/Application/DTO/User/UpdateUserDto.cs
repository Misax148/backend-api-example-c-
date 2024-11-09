namespace MovieApi.Core.src.Application.DTO.User;

public record UpdateUserDto(
    string Username,
    string Email,
    string Role
);