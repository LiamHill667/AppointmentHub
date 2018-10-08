using AppointmentHub.Core.Models;
using System.Collections.Generic;

namespace AppointmentHub.Core.Repositories
{
    public interface IAppointmentRepository : IGenericRepository<Appointment>
    {
        Appointment GetById(int id);
        IEnumerable<Appointment> GetMyAppointments(string userId);
        IEnumerable<Appointment> GetMyAppointmentsPaged(string userId, int pageNum, out int totalPages, string query = null, int pageSize = 12);
    }
}
