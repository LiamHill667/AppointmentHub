using AppointmentHub.Core.Extensions;
using AppointmentHub.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AppointmentHub.Core.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AppointmentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Appointment> GetAppointmentsPaged(string userId, int pageNum, out int totalPages, string query)
        {
            totalPages = 0;

            if (query == null || DateTime.TryParse(query, out DateTime parsed))
                return _unitOfWork.Appointment.GetMyAppointmentsPaged(userId, pageNum, out totalPages, query);
            else
                return new List<Appointment>();
        }

        public IEnumerable<TimeSpan> GetAppointmentStartTimes(UserAvailability availability)
        {
            var startTime = availability.GetStartTime();

            return new TimeSpan().HoursOfTheDay()
                .Where(t => t >= startTime);
        }

        public IEnumerable<TimeSpan> GetAppointmentEndTimes(UserAvailability availability)
        {
            var startTime = availability.GetStartTime();
            var endTime = availability.GetEndTime();

            return new TimeSpan().HoursOfTheDay()
                .Where(t => t > startTime && t <= endTime);
        }

        public IEnumerable<AppointmentType> GetAppointmentTypes()
        {
            return _unitOfWork.AppointmentType.GetAll();
        }

        public ServiceResult BookAppointment(string subject, DateTime dateTime, TimeSpan timeSpan, string requesteeId, int typeId, int availabilityId)
        {
            var availability = _unitOfWork.UserAvailability.GetById(availabilityId);

            if (availability == null)
                return new ServiceResult(false, "User Availability not Found");

            var appointment = new Appointment()
            {
                Subject = subject,
                DateTime = dateTime,
                TimeSpan = timeSpan,
                RequestedId = availability.UserId,
                RequesteeId = requesteeId,
                TypeId = typeId
            };

            ManageBooking(appointment, availability);

            _unitOfWork.Complete();

            return new ServiceResult(true);

        }

        internal void ManageBooking(Appointment appointment, UserAvailability availability)
        {

            if (availability.DateTime == appointment.DateTime && availability.TimeSpan == appointment.TimeSpan)
            {
                //delete the UserAvailability the appointment is the entirity
                _unitOfWork.UserAvailability.Delete(availability);
            }
            else if (availability.DateTime == appointment.DateTime)
            {
                //start of the slot change slot to start at the end of the appointment and length is the difference between the lengths
                availability.DateTime = availability.DateTime.Add(appointment.TimeSpan);
                availability.TimeSpan = availability.TimeSpan.Subtract(appointment.TimeSpan);

            }
            else if (availability.DateTime.TimeOfDay.Add(availability.TimeSpan) == appointment.DateTime.TimeOfDay.Add(appointment.TimeSpan))
            {
                //the appointment runs to the end of the availability slot so change the endtime of the slot to the start appointment
                availability.TimeSpan = availability.TimeSpan.Subtract(appointment.TimeSpan);
            }
            else
            {
                var originalTimeSpan = availability.TimeSpan;
                availability.TimeSpan = appointment.DateTime.TimeOfDay.Subtract(availability.DateTime.TimeOfDay);
                //the appointment occurs inside the availability time span therefore a new availability will need to be created 
                //and original updated to represent the sub availability created by the appointment

                var newAvailability = new UserAvailability()
                {
                    UserId = availability.UserId,
                    DateTime = appointment.DateTime.Add(appointment.TimeSpan),
                    TimeSpan = originalTimeSpan.Subtract(availability.TimeSpan.Add(appointment.TimeSpan))
                };

                _unitOfWork.UserAvailability.Add(newAvailability);
            }

            _unitOfWork.Appointment.Add(appointment);
        }

        public ServiceResult CancelAppointment(int appointmentId, string userId)
        {
            var appointment = _unitOfWork.Appointment.GetById(appointmentId);

            if (appointment == null || appointment.RequesteeId != userId)
                return new ServiceResult(false, $"Appointment cannot be found appointmentId: { appointmentId } or not authorized to access appointment");

            appointment.Cancel();
            _unitOfWork.Complete();

            return new ServiceResult(true);
        }
    }
}