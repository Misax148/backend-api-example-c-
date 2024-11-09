using FluentValidation;
using MovieApi.Core.src.Application.DTO.User;

namespace MovieApi.API.src.Validators;

public class CreateUserValidator : AbstractValidator<CreateUserDto>
{
    private readonly string[] _allowedRoles = { "User", "Admin" };

    public CreateUserValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username is required")
            .Length(3, 50).WithMessage("Username must be between 3 and 50 characters");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format")
            .MaximumLength(100).WithMessage("Email must not exceed 100 characters");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters")
            .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter")
            .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter")
            .Matches("[0-9]").WithMessage("Password must contain at least one number")
            .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character");

        RuleFor(x => x.Role)
            .Must(role => string.IsNullOrEmpty(role) || _allowedRoles.Contains(role))
            .WithMessage($"Role must be one of: {string.Join(", ", _allowedRoles)}");
    }
}
