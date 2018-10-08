using AppointmentHub.Core.Models;
using AppointmentHub.Core.Repositories;
using AppointmentHub.Persistence.Extensions;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace AppointmentHub.Persistence.Repositories
{
    public class AppointmentRepository : GenericRepository<Appointment>, IAppointmentRepository
    {
        public AppointmentRepository(IApplicationDbContext context) : base(context)
        {
        }

        public Appointment GetById(int id)
        {
            return _context.Appointments
                .Include(a => a.Requested)
                .Include(a => a.Requestee)
                .SingleOrDefault(a => a.Id == id);
        }

        public IEnumerable<Appointment> GetMyAppointments(string userId)
        {
            return _context.Appointments
                .Include(a => a.Requested)
                .Where(a => a.RequesteeId == userId && a.IsCanceled == false)
                .ToList();
        }

        public IEnumerable<Appointment> GetMyAppointmentsPaged(string userId, int pageNum, out int totalPages, string query = null, int pageSize = 12)
        {
            var appointments = _context.Appointments
                .Include(a => a.Requested)
                .Where(a => a.RequesteeId == userId && a.IsCanceled == false);

            if (!String.IsNullOrWhiteSpace(query))
            {
                var parsed = DateTime.TryParse(query, out DateTime parsedDate);

                if (!parsed)
                    throw (new FormatException("searchTerm string is not in parsable datetime format"));

                appointments = appointments
                    .Where(a => a.DateTime.TruncateTime() == parsedDate.TruncateTime());
            }

            //
            totalPages = appointments.Count() / pageSize;

            return appointments.OrderBy(a => a.DateTime).Skip(pageNum * pageSize).Take(pageSize).ToList();
        }
    }
}