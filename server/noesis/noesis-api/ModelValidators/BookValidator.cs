using System;
using FluentValidation;
using noesis_api.Models;

namespace noesis_api.ModelValidators
{
    public class BookValidator : AbstractValidator<Book>
    {
        public BookValidator()
        {
            RuleFor(b => b.Title).MinimumLength(2).WithMessage("Ttitle must have at least 2 characaters!");
            RuleFor(b => b.Year).LessThanOrEqualTo(DateTime.UtcNow.Year).WithMessage("Book cannot be published in the future!");
        }
    }
}
