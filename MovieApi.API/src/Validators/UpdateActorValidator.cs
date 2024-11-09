using FluentValidation;
using MovieApi.Core.src.Application.DTO.Actor;

namespace MovieApi.API.src.Validators;

public class UpdateActorValidator : AbstractValidator<UpdateActorDto>
{
    public UpdateActorValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .Length(1, 255).WithMessage("Name must be between 1 and 255 characters");

        RuleFor(x => x.Biography)
            .NotEmpty().WithMessage("Biography is required");

        RuleFor(x => x.Birthdate)
            .NotEmpty().WithMessage("Birthdate is required")
            .LessThan(DateTime.UtcNow).WithMessage("Birthdate cannot be in the future");
    }
}
