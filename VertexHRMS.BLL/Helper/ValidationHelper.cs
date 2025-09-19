using System.Text.RegularExpressions;

namespace VertexHRMS.BLL.Helper
{
    public static class ValidationHelper
    {
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                return false;

            try
            {
                var regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase);
                return regex.IsMatch(email);
            }
            catch
            {
                return false;
            }
        }

        public static bool IsValidPhone(string phone)
        {
            if (string.IsNullOrEmpty(phone))
                return false;

            // Remove all non-digit characters
            var digitsOnly = Regex.Replace(phone, @"\D", "");

            // Check if it has 10-15 digits (international format)
            return digitsOnly.Length >= 10 && digitsOnly.Length <= 15;
        }

        public static bool IsValidName(string name)
        {
            if (string.IsNullOrEmpty(name) || name.Trim().Length < 2)
                return false;

            // Only letters, spaces, hyphens, and apostrophes
            var regex = new Regex(@"^[a-zA-Z\s\-']+$");
            return regex.IsMatch(name.Trim());
        }

        public static string FormatPhoneNumber(string phone)
        {
            if (string.IsNullOrEmpty(phone))
                return phone;

            var digitsOnly = Regex.Replace(phone, @"\D", "");

            if (digitsOnly.Length == 10)
            {
                return $"({digitsOnly.Substring(0, 3)}) {digitsOnly.Substring(3, 3)}-{digitsOnly.Substring(6, 4)}";
            }

            return phone; // Return original if not standard format
        }
    }
}
