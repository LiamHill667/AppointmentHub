using AppointmentHub.Core.Models;
using System;
using System.Collections.Generic;

namespace AppointmentHub.Core.Services
{
    public class UserAvailabilityService : IUserAvailabilityService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserAvailabilityService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public ServiceResult CreateAvailability(UserAvailability userAvailability)
        {
            _unitOfWork.UserAvailability.Add(userAvailability);
            _unitOfWork.Complete();

            return new ServiceResult(true);
        }

        public ServiceResult DeleteAvailability(int id, string userId)
        {
            var userAvailability = GetAvailability(id);

            if (userAvailability == null)
                return new ServiceResult(false, "Availability not found.");

            if (userAvailability.UserId != userId)
                return new ServiceResult(false, "User does not have permission.");

            _unitOfWork.UserAvailability.Delete(userAvailability);
            _unitOfWork.Complete();

            return new ServiceResult(true);
        }

        public UserAvailability GetAvailability(int id)
        {
            return _unitOfWork.UserAvailability.GetById(id);
        }

        public IEnumerable<UserAvailability> GetFutureBookableAvailabilitiesPaged(string userId, int pageNum, out int totalPages, string query)
        {
            totalPages = 0;

            if (query == null || DateTime.TryParse(query, out DateTime parsed))
                return _unitOfWork.UserAvailability.GetFutureBookableUserAvailabilitiesPaged(userId, pageNum, out totalPages, query);
            else
                return new List<UserAvailability>();
        }

        public IEnumerable<UserAvailability> GetFutureUserAvailabilitiesPaged(string userId, int pageNum, out int totalPages, string query)
        {
            totalPages = 0;

            if (query == null || DateTime.TryParse(query, out DateTime parsed))
                return _unitOfWork.UserAvailability.GetFutureUserAvailabilitiesPaged(userId, pageNum, out totalPages, query);
            else
                return new List<UserAvailability>();
        }

        public ServiceResult UpdateAvailability(UserAvailability userAvailability)
        {
            _unitOfWork.UserAvailability.Update(userAvailability);
            _unitOfWork.Complete();

            return new ServiceResult(true);
        }
    }
}