using AppointmentHub.Core.ViewModels;
using FluentValidation;

namespace AppointmentHub.Core.Validators
{
    public class ManageUserFormValidator : AbstractValidator<ManageUserViewModel>
    {
        public ManageUserFormValidator()
        {
            RuleFor(u => u.Name)
                .NotEmpty();

            RuleFor(u => u.Email)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .EmailAddress();

            When(u => string.IsNullOrEmpty(u.Id), () =>
            {
                RuleFor(u => u.Password)
                   .Cascade(CascadeMode.StopOnFirstFailure)
                   .NotEmpty()
                   .MinimumLength(6)
                   .MaximumLength(100);
            });

        }
    }
}