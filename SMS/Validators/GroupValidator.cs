using FluentValidation;
using SMS.Models;

namespace SMS.Validators
{
    public class GroupValidator : AbstractValidator<Group>
    {
        public GroupValidator()
        {
            RuleFor(g => g.Name).NotEmpty().WithMessage("Please fill out the name of group!");
            RuleFor(g => g.Description).NotEmpty().WithMessage("Please fill out the description!");
            RuleFor(g => g.SubjectId).NotEmpty().WithMessage("Please select a subject!");
        }
    }
}
