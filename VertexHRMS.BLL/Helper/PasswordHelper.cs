using System.Security.Cryptography;
using System.Text;

namespace VertexHRMS.BLL.Helper
{
    public static class PasswordHelper
    {
        public static bool IsStrongPassword(string password)
        {
            if (string.IsNullOrEmpty(password) || password.Length < 6)
                return false;

            bool hasUpperCase = password.Any(char.IsUpper);
            bool hasLowerCase = password.Any(char.IsLower);
            bool hasDigit = password.Any(char.IsDigit);
            bool hasSpecialChar = password.Any(ch => !char.IsLetterOrDigit(ch));

            return hasUpperCase && hasLowerCase && hasDigit;
        }

        public static string GenerateRandomPassword(int length = 12)
        {
            const string validChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*?_-";
            var random = new Random();
            var result = new StringBuilder();

            for (int i = 0; i < length; i++)
            {
                result.Append(validChars[random.Next(validChars.Length)]);
            }

            return result.ToString();
        }
    }
}
