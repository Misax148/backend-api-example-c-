namespace MovieApi.Core.src.Application.DTO.User;

public record CreateUserDto(
    string Username,
    string Email,
    string Password,
    string Role = "User"
);
