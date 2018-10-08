using AppointmentHub.Core.ViewModels;
using FluentValidation;
using System;

namespace AppointmentHub.Core.Validators
{
    public class BookFormValidator : AbstractValidator<BookFormViewModel>
    {
        public BookFormValidator()
        {
            RuleFor(b => b.Subject)
               .NotEmpty()
               .WithMessage("Subject should not be empty");

            RuleFor(b => b.Subject)
                .MaximumLength(255)
                .WithMessage("Subject should not be longer than 255 characters");

            RuleFor(b => b.TimeSpan)
                .GreaterThanOrEqualTo(new TimeSpan(1, 0, 0))
                .WithMessage("Length must be at least 1 hour");

            RuleFor(b => b.EndTime)
                .Must((b, EndTime) => ValidAppointmentLength(b))
                .WithMessage("The appointment finishes later than the availability");

            RuleFor(b => b.StartTime)
                .Must((b, StartTime) =>
                ValidAppointmentStartTime(StartTime, b.Availability))
                .WithMessage("The appointment does not start within the available time");

        }

        /// <summary>
        /// Checking the time is inclusive between availability
        /// start and end time
        /// </summary>
        /// <param name="time"></param>
        /// <param name="availability"></param>
        /// <returns></returns>
        private bool ValidAppointmentStartTime(TimeSpan StartTime, UserAvailabilityViewModel availability)
        {
            return StartTime >= availability.DateTime.TimeOfDay
                && StartTime <= availability.DateTime.TimeOfDay.Add(availability.TimeSpan);
        }

        /// <summary>
        /// Checking the appointment does not finish later than the availability
        /// allows
        /// </summary>
        /// <param name="appointment"></param>
        /// <param name="availability"></param>
        /// <returns></returns>
        private bool ValidAppointmentLength(BookFormViewModel viewModel)
        {
            return viewModel.DateTime.Add(viewModel.TimeSpan)
                .CompareTo(viewModel.Availability.DateTime.Add(viewModel.Availability.TimeSpan)) < 1;
        }
    }
}