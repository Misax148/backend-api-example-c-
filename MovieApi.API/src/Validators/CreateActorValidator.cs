using FluentValidation;
using MovieApi.Core.src.Application.DTO.Actor;

namespace MovieApi.API.src.Validators;

public class CreateActorValidator : AbstractValidator<CreateActorDto>
{
    public CreateActorValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .Length(1, 255).WithMessage("Name must be between 1 and 255 characters");

        RuleFor(x => x.Biography)
            .NotEmpty().WithMessage("Biography is required");

        RuleFor(x => x.Birthdate)
            .NotEmpty().WithMessage("Birthdate is required")
            .LessThan(DateTime.UtcNow).WithMessage("Birthdate cannot be in the future")
            .Must(BeAValidAge).WithMessage("Actor must be at least 1 year old");
    }

    private bool BeAValidAge(DateTime Birthdate)
    {
        var age = DateTime.UtcNow.Year - Birthdate.Year;
        return age >= 1;
    }
}
