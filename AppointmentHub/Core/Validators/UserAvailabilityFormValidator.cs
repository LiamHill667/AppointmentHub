using AppointmentHub.Core.ViewModels;
using FluentValidation;
using System;

namespace AppointmentHub.Core.Validators
{
    public class AvailabilityFormValidator : AbstractValidator<AvailabilityFormViewModel>
    {
        public AvailabilityFormValidator()
        {

            RuleFor(a => a.Date)
                .NotNull();

            When(a => !String.IsNullOrEmpty(a.Date), () =>
            {
                RuleFor(a => a.Date)
              .StringValidDateTimeFormat("dd/MM/yyyy");
            });

            RuleFor(a => a.TimeSpan)
               .GreaterThanOrEqualTo(new TimeSpan(1, 0, 0))
               .WithMessage("Duration must be at least 1 hour");

            When(a => !String.IsNullOrEmpty(a.Date) &&
                      DateTime.TryParse(a.Date, out DateTime parsedDate),
                      () =>
            {
                RuleFor(a => a.Date)
               .Must(d => DateTime.Parse(d) >= DateTime.Now.Date)
               .WithMessage("Date must be for future appointments");

                RuleFor(a => a.DateTime)
               .Must((availability, dateTime) => OneDayPerAppointment(dateTime, availability.TimeSpan))
               .WithMessage("Appointment cannot got into the next day");
            });



        }

        private bool OneDayPerAppointment(DateTime dateTime, TimeSpan timeSpan)
        {
            var startDate = dateTime.Date;
            var endDate = dateTime.Add(timeSpan).Date;

            return startDate.CompareTo(endDate) == 0;
        }
    }
}