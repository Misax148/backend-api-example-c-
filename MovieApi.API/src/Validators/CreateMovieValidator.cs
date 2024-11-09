using FluentValidation;
using MovieApi.Core.src.Application.DTO.Movie;

namespace MovieApi.API.src.Validators;

public class CreateMovieValidator : AbstractValidator<CreateMovieDto>
{
    public CreateMovieValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .Length(1, 255).WithMessage("Title must be between 1 and 255 characters");

        RuleFor(x => x.ReleaseDate)
            .NotEmpty().WithMessage("Release date is required")
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Release date cannot be in the future");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required");

        RuleFor(x => x.Genre)
            .NotEmpty().WithMessage("Genre is required")
            .Length(1, 50).WithMessage("Genre must be between 1 and 50 characters");

        RuleFor(x => x.Rating)
            .InclusiveBetween(0, 10).WithMessage("Rating must be between 0 and 10");
    }
}
