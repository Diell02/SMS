using FluentValidation;
using SMS.Models;

namespace SMS.Validators
{
    public class SubjectValidator : AbstractValidator<Subject>
    {
        public SubjectValidator()
        {
            RuleFor(g => g.Name).NotEmpty().WithMessage("Please fill out the name of group!");
            RuleFor(g => g.Description).NotEmpty().WithMessage("Please fill out the description!");
        }
    }
}
