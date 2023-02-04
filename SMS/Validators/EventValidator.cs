using FluentValidation;
using SMS.Models;

namespace SMS.Validators
{
    public class EventValidator : AbstractValidator<Event>
    {
        public EventValidator()
        {
            RuleFor(g => g.Name).NotEmpty().WithMessage("Please fill out the name of group!");
            RuleFor(g => g.Description).NotEmpty().WithMessage("Please fill out the description!");
            RuleFor(g => g.CategoryId).NotEmpty().WithMessage("Please select a subject!");
            RuleFor(e => e.Created).NotEmpty().WithMessage("Please specify a date")
                .Must(ValidDate).WithMessage("Please specify a Valid Date");
        }

        protected bool ValidDate(DateTime eventDate)
        {
            DateTime currentDate = DateTime.Now;
            if (eventDate < currentDate)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
