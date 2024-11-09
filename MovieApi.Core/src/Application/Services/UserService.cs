using FluentValidation;
using MovieApi.Core.src.Application.DTO.User;
using MovieApi.Core.src.Domain.Entities;
using MovieApi.Core.src.Domain.Exceptions;
using MovieApi.Core.src.Domain.Interfaces.Repositories;
using MovieApi.Core.src.Domain.Interfaces.Services;

namespace MovieApi.Core.src.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IValidator<CreateUserDto> _createUserValidator;
    private readonly IValidator<UpdateUserDto> _updateUserValidator;

    public UserService(
        IUserRepository userRepository,
        IValidator<CreateUserDto> createUserValidator,
        IValidator<UpdateUserDto> updateUserValidator)
    {
        _userRepository = userRepository;
        _createUserValidator = createUserValidator;
        _updateUserValidator = updateUserValidator;
    }
    
    public async Task<UserDto> CreateUserAsync(CreateUserDto userDto)
    {
        var validationResult = await _createUserValidator.ValidateAsync(userDto);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        if (!await IsEmailUniqueAsync(userDto.Email))
        {
            throw new BadRequestException("Email is already in use");
        }

        var user = MapToEntity(userDto);
        var created = await _userRepository.CreateAsync(user);
        
        if (!created)
        {
            throw new InternalServerErrorException("Failed to create user");
        }

        return await GetUserByIdAsync(user.Id);
    }

    public async Task<bool> DeleteUserAsync(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
        {
            throw new NotFoundException($"User with ID {id} not found");
        }

        return await _userRepository.DeleteAsync(id);
    }

    public async Task<UserDto> GetUserByEmailAsync(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new BadRequestException("Email cannot be empty");
        }

        var user = await _userRepository.GetUserByEmailAsync(email);
        if (user == null)
        {
            throw new NotFoundException($"User with email {email} not found");
        }

        return MapToDto(user);
    }

    public async Task<UserDto> GetUserByIdAsync(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
        {
            throw new NotFoundException($"User with ID {id} not found");
        }

        return MapToDto(user);
    }

    public async Task<bool> IsEmailUniqueAsync(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new BadRequestException("Email cannot be empty");
        }

        return await _userRepository.IsEmailUniqueAsync(email);
    }

    public async Task<UserDto> UpdateUserAsync(Guid id, UpdateUserDto updateUserDto)
    {
        var validationResult = await _updateUserValidator.ValidateAsync(updateUserDto);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
        {
            throw new NotFoundException($"User with ID {id} not found");
        }

        // Verificar email único solo si cambió
        if (user.Email != updateUserDto.Email && !await IsEmailUniqueAsync(updateUserDto.Email))
        {
            throw new BadRequestException("Email is already in use");
        }

        UpdateEntityFromDto(user, updateUserDto);
        var updated = await _userRepository.UpdateAsync(user);
        
        if (!updated)
        {
            throw new InternalServerErrorException("Failed to update user");
        }

        return await GetUserByIdAsync(id);
    }

    private User MapToEntity(CreateUserDto dto)
    {
        return new User
        {
            Id = Guid.NewGuid(),
            Username = dto.Username,
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Role = dto.Role
        };
    }

    private void UpdateEntityFromDto(User user, UpdateUserDto dto)
    {
        user.Username = dto.Username;
        user.Email = dto.Email;
        user.Role = dto.Role;
    }

    private UserDto MapToDto(User user)
    {
        return new UserDto(
            user.Id,
            user.Username,
            user.Email,
            user.Role
        );
    }
}
