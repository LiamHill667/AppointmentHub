using AppointmentHub.Controllers;
using AppointmentHub.Core.Extensions;
using AppointmentHub.Core.Validators;
using FluentValidation.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace AppointmentHub.Core.ViewModels
{
    [Validator(typeof(AvailabilityFormValidator))]
    public class AvailabilityFormViewModel
    {
        public int Id { get; set; }

        public string Date { get; set; }

        [Display(Name = "Start Time")]
        public TimeSpan StartTime { get; set; }

        [Display(Name = "End Time")]
        public TimeSpan EndTime { get; set; }

        public TimeSpan TimeSpan => EndTime.Subtract(StartTime);

        public DateTime DateTime => DateTime.Parse($"{Date} {StartTime}");

        public List<TimeSpan> SelectableStartTimes => TimeSpan.HoursOfTheDay();

        public List<TimeSpan> SelectableEndTimes => TimeSpan.HoursOfTheDay().Where(t => t > StartTime).ToList();

        public string Heading
        {
            get
            {
                return Id == 0 ? "Create" : "Edit";
            }
        }

        public string Action
        {
            get
            {
                Expression<Func<AvailabilityController, ActionResult>> update =
                    (c => c.Update(this));

                Expression<Func<AvailabilityController, ActionResult>> create =
                    (c => c.Create(this));

                var action = (Id != 0) ? update : create;
                return (action.Body as MethodCallExpression).Method.Name;
            }
        }

        public AvailabilityFormViewModel()
        {
            Date = DateTime.Now.ToString("dd/MM/yyyy");
            EndTime = new TimeSpan(DateTime.Now.Hour + 1, 0, 0);
            StartTime = new TimeSpan(DateTime.Now.Hour, 0, 0);
        }
    }
}