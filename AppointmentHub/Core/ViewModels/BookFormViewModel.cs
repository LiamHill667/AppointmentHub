using AppointmentHub.Core.Validators;
using FluentValidation.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AppointmentHub.Core.ViewModels
{
    [Validator(typeof(BookFormValidator))]
    public class BookFormViewModel
    {
        public UserAvailabilityViewModel Availability { get; set; }
        public IEnumerable<AppointmentTypeViewModel> Types { get; set; }

        public string Subject { get; set; }

        public AppointmentTypeViewModel Type { get; set; }

        public string Date => Availability.DateTime.ToString("dd/MM/yyyy");

        [Display(Name = "Start Time")]
        public TimeSpan StartTime { get; set; }

        [Display(Name = "End Time")]
        public TimeSpan EndTime { get; set; }

        public TimeSpan TimeSpan => EndTime.Subtract(StartTime);

        public DateTime DateTime => DateTime.Parse($"{Date} {StartTime}");

        public IEnumerable<TimeSpan> SelectableStartTimes { get; set; }

        public IEnumerable<TimeSpan> SelectableEndTimes { get; set; }

    }
}