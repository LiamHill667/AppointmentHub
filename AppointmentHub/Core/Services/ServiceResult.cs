namespace AppointmentHub.Core.Services
{
    public class ServiceResult
    {
        public bool Succeeded { get; private set; }
        public string ErrorMessage { get; private set; }

        public ServiceResult(bool succeeded, string errorMessage = null)
        {
            Succeeded = succeeded;
            ErrorMessage = errorMessage;
        }
    }
}