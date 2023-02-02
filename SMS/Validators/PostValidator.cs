using FluentValidation;
using SMS.Models;

namespace SMS.Validators
{
    public class PostValidator : AbstractValidator<Post>
    {
        public PostValidator()
        {
            RuleFor(g => g.Title).NotEmpty().WithMessage("Please fill out the title of post!");
            RuleFor(g => g.Description).NotEmpty().WithMessage("Please fill out the description!");
        }
    }
}
