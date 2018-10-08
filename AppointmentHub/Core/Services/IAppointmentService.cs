using AppointmentHub.Core.Models;
using System;
using System.Collections.Generic;

namespace AppointmentHub.Core.Services
{
    public interface IAppointmentService
    {
        IEnumerable<Appointment> GetAppointmentsPaged(string userId, int pageNum, out int totalPages, string query);
        IEnumerable<AppointmentType> GetAppointmentTypes();
        IEnumerable<TimeSpan> GetAppointmentStartTimes(UserAvailability availability);
        IEnumerable<TimeSpan> GetAppointmentEndTimes(UserAvailability availability);
        ServiceResult BookAppointment(string subject, DateTime dateTime, TimeSpan timeSpan, string requesteeId, int typeId, int availabilityId);
        ServiceResult CancelAppointment(int appointmentId, string userId);

    }
}
