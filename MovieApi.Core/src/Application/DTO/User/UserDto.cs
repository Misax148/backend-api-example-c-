namespace MovieApi.Core.src.Application.DTO.User;

public record UserDto(
    Guid Id,
    string? Username,
    string? Email,
    string? Role
);
