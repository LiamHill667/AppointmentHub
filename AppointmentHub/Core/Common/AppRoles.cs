using System.Collections.Generic;
using System.Linq;

namespace AppointmentHub.Core.Common
{
    public static class AppRoles
    {
        public const string Admin = "Admin";
        public const string Contact = "Contact";
        public const string User = "User";


        public static IEnumerable<string> GetRoleNames()
        {
            return typeof(AppRoles).GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static)
                 .Where(fi => fi.IsLiteral && !fi.IsInitOnly)
                 .Select(fi => fi.Name);
        }
    }
}